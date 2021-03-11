using UnityEngine;
using UnityEngine.Events;
using Unity.Mathematics;
using Unity.Collections;
using Game.Voxels.Editor.Data;
using UnityEngine.InputSystem;

namespace Game.Voxels.Editor
{
    public class PlayerController : MonoBehaviour, IGameData
    {
        [SerializeField]
        private LayerMask mask = 0;

        public PlayerInput input { get; private set; }

        [SerializeField]
        private Transform camTr = null;
        [SerializeField]
        private Camera cam = null;

        private CharacterController character;
        private Transform tr;
        private PlayerInventory playerInventory;

        [SerializeField]
        private float moveSpeed = 5f;
        [SerializeField]
        private float sensitivity = 120f;
        [SerializeField]
        private float interactDistance = 5f;
        [SerializeField]
        private float maxAngle = 80f;
        [SerializeField]
        private float maxAllowedOffset = 100f;

        private float forwardInput;
        private float upInput;
        private float rightInput;
        private Vector2 camInput;

        private Vector2 camStartPos, camPos;

        public UnityEvent<VoxelPlaceEventArgs> onVoxelPlace { get; } = new UnityEvent<VoxelPlaceEventArgs>();
        public UnityEvent<VoxelBreakEventArgs> onVoxelBreak { get; } = new UnityEvent<VoxelBreakEventArgs>();

        private void Awake()
        {
            tr = transform;
            playerInventory = GetComponent<PlayerInventory>();
            if (character == null)
                character = GetComponent<CharacterController>();

            input = new PlayerInput();

            SetupPlayerInput();
            SetupUIInput();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void OnEnable()
        {
            input.Enable();
        }

        private void OnDisable()
        {
            input.Disable();
        }

        private void OnDestroy()
        {
            input.Dispose();
        }

        private void Update()
        {
            MovePlayer();
            RotatePlayer();
        }

        private void LateUpdate()
        {
            RotateCamera();
        }

        public void OnLoad(string json)
        {
            character = GetComponent<CharacterController>();
            character.enabled = false;

            PlayerControllerData data = JsonUtility.FromJson<PlayerControllerData>(json);

            transform.position = data.position;
            transform.eulerAngles = data.playerRotation;
            camTr.eulerAngles = data.cameraRotation;

            character.enabled = true;
        }

        public string OnSave()
        {
            return JsonUtility.ToJson(new PlayerControllerData()
            {
                position = tr.position,
                playerRotation = tr.eulerAngles,
                cameraRotation = camTr.eulerAngles
            });
        }

        private void SetupUIInput()
        {
            input.UI.TogglePlayer.performed += _ =>
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;

                input.Player.Enable();
                input.UI.Disable();
            };
        }

        #region Player Methods
        private void SetupPlayerInput()
        {
            var bindingGroup = Application.isMobilePlatform ? input.AndroidScheme.bindingGroup : input.KeyboardandMouseScheme.bindingGroup;
            input.bindingMask = InputBinding.MaskByGroup(bindingGroup);

            input.Player.Zmove.performed += ctx => forwardInput = ctx.ReadValue<float>();
            input.Player.Zmove.canceled += _ => forwardInput = 0f;

            input.Player.Ymove.performed += ctx => upInput = ctx.ReadValue<float>();
            input.Player.Ymove.canceled += _ => upInput = 0f;

            input.Player.Xmove.performed += ctx => rightInput = ctx.ReadValue<float>();
            input.Player.Xmove.canceled += _ => rightInput = 0f;

            input.Player.CameraInput.performed += ctx => camInput = ctx.ReadValue<Vector2>();
            input.Player.CameraInput.canceled += _ => camInput = Vector2.zero;

            input.Player.BreakVoxel.performed += _ => BreakVoxel();
            input.Player.PlaceVoxel.performed += _ => PlaceVoxel();

            input.Player.CameraPosition.started += ctx => camStartPos = ctx.ReadValue<Vector2>();
            input.Player.CameraPosition.performed += ctx => camPos = ctx.ReadValue<Vector2>();

            input.Player.SwitchItem.performed += ctx => GetComponent<PlayerInventory>().SwitchHotbarItemByInput(ctx.ReadValue<float>());

            input.Player.ToggleUI.performed += _ =>
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;

                input.Player.Disable();
                input.UI.Enable();
            };
        }

        private void MovePlayer()
        {
            Vector3 move = Vector3.ClampMagnitude(new Vector3(rightInput, upInput, forwardInput), 1f) * moveSpeed * Time.deltaTime;
            character.Move(tr.rotation * move);
        }

        private void RotatePlayer()
        {
            float angle = camInput.x * sensitivity * Time.deltaTime;
            tr.Rotate(Vector3.up, angle);
        }

        private void RotateCamera()
        {
            float angle = camTr.localEulerAngles.x - camInput.y * sensitivity * Time.deltaTime;
            angle = math.select(math.max(angle, 360f - maxAngle), math.min(angle, maxAngle), angle < 180f);
            camTr.localEulerAngles = new Vector3(angle, 0f, 0f);
        }

        private bool CanInteract() => (camStartPos - camPos).sqrMagnitude < maxAllowedOffset;

        private void PlaceVoxel()
        {
            if (!CanInteract())
                return;

            Ray ray = cam.ViewportPointToRay(Vector2.one / 2f);
            //Ray ray = cam.ScreenPointToRay(camInput);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, mask))
                if (hit.transform.TryGetComponent(out VoxelWorldEditor editor))
                {
                    Vector3 pos = editor.component.transform.InverseTransformPoint(hit.point) + hit.normal * .5f;
                    int3 voxelPos = (int3)math.floor(pos);

                    VoxelPlaceEventArgs args = new VoxelPlaceEventArgs() { position = voxelPos, selectedItem = playerInventory.selectedItem };
                    onVoxelPlace.Invoke(args);

                    if (args.handleCode == VoxelPlaceEventArgs.HandleCode.None)
                    {
                        int3 minBounds = math.select(int3.zero, new int3(1, 1, 1), voxelPos < int3.zero);
                        int3 maxBounds = math.select(int3.zero, new int3(1, 1, 1), voxelPos == editor.component.world.size);

                        bool resize = math.any((minBounds | maxBounds) != int3.zero);
                        if (resize)
                        {
                            editor.component.ResizeWorld(minBounds, maxBounds);
                            voxelPos += minBounds;
                        }

                        editor.PlaceVoxel(voxelPos, args.selectedItem.content, args.selectedItem.health);
                        VoxelWorldBuilder.BuildVoxelWorld(editor.component, voxelPos, voxelPos);

                        if (resize)
                        {
                            editor.ship.modules.ResizeMask(editor.component.world.size);
                            editor.ship.modules.Update();
                        }
                    }
                    else
                        editor.editorUI.ShowWarningMessage(VoxelPlaceEventArgs.cancelWarningMessages[(int)args.handleCode - 1]);
                }
        }

        private void BreakVoxel()
        {
            if (!CanInteract())
                return;

            Ray ray = cam.ViewportPointToRay(Vector2.one / 2f);
            //Ray ray = cam.ScreenPointToRay(camInput);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, mask))
                if (hit.transform.TryGetComponent(out VoxelWorldEditor editor))
                {
                    Vector3 pos = editor.component.transform.InverseTransformPoint(hit.point) - hit.normal / 2f;
                    int3 voxelPos = (int3)math.floor(pos);

                    VoxelBreakEventArgs args = new VoxelBreakEventArgs() { position = voxelPos };
                    onVoxelBreak.Invoke(args);

                    if (args.handleCode == VoxelBreakEventArgs.HandleCode.None)
                    {
                        bool3 trimPos = voxelPos == (editor.component.world.size - 1), trimNeg = voxelPos == int3.zero;
                        editor.BreakVoxel(voxelPos);

                        bool resize = false;

                        if (math.any(trimPos) || math.any(trimNeg))
                        {
                            NativeArray<int3> result = VoxelEditorTools.TrimWorld(editor.component.world);

                            resize = math.any((result[0] | result[1]) != int3.zero);
                            if (resize)
                            {
                                editor.component.ResizeWorld(-result[0], -result[1]);
                                editor.ship.modules.ResizeMask(editor.component.world.size);
                                editor.ship.modules.Update();
                            }

                            result.Dispose();
                        }

                        if (!resize)
                            VoxelWorldBuilder.BuildVoxelWorld(editor.component, voxelPos, voxelPos);
                    }
                    else
                        editor.editorUI.ShowWarningMessage(VoxelBreakEventArgs.cancelWarningMessages[(int)args.handleCode - 1]);
                }
        }
        #endregion
    }
}

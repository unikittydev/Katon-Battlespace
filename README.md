# Katon-Battlespace

![Изображение с корабликами](https://user-images.githubusercontent.com/75719391/110838119-25363b00-82b3-11eb-9d43-30e02797f9b9.png)

Репозиторий проекта **Katon: Battlespace** и его визитная карточка.

## Об игре
> Я вам такой корабль построю, вы все попадаете!
> 
> Ну в смысле, от удивления
> 
*Katon: Battlespace* - игра о полётах в космосе и о строительстве кораблей. Пусть игра и ограничивает свободу художнику своей угловатостью и малым количеством запчастей (16 цветов на каждый из 16 материалов), игрок всё ещё может потренироваться в конструировании корабля с технической точки зрения, ведь от расстановки деталей зависит то, какой корабль в итоге получится и какое приключение этот игрок на нём испытает - долгое и наполненное красками или очень кратковременное...

[Дизайн-документ](https://docs.google.com/document/d/1HGFJKazQLUXh3KHtl_5YbpAp1U28bYtq7fA64EflLo4/edit?usp=sharing) проекта можно изучить в Google Docs.

Краткий видеообзор проекта ([Плейлист](https://www.youtube.com/watch?v=IGB8aBQnkII&list=PLnRh3_Lo2vupQVoHEfJIGooQYXL504a_A) на YouTube):
- [Строительство корабля](https://www.youtube.com/watch?v=IGB8aBQnkII);
- [Управление и полёт](https://www.youtube.com/watch?v=JcEAleAuhk8);
- [Повреждения и взрывы](https://www.youtube.com/watch?v=x6HCjHZyGGk);
- [Столкновения](https://www.youtube.com/watch?v=Efzp1ict7Ks);
- [Кончилось топливо](https://www.youtube.com/watch?v=A_3YdXi1JRw).

Достигнутый результат пусть и меньше ожидаемого (изначально цели были гораздо амбициознее, но со временем пришло осознание того, что один разработчик не может сделать всё что угодно), однако его уже достаточно, чтобы гордиться своим трудом.

## Об авторе
Меня зовут Русаков Владислав, я студент второго курса Государственного Университета "Дубна". Учусь на направлении "Информатика и Вычислительная Техника".
Пришёл в *Unity* в мае 2018 года (тогда же начал изучать ЯП *C#*), и с тех пор занимаюсь разработкой игр.

*Автором идеи и единственным разработчиком игры на протяжении всего цикла разработки являюсь* **я**.

## Прогресс
*Внимание*! Данный продукт всё ещё находится на этапе разработки. Во время игры Вы можете столкнуться с различного рода багами и недочётами. О любых проблемах, связанных с некорректной работой игры, вы можете сообщить в разделе *Issues* или по почте 777ruspacan777@gmail.com.

### Что такое Катон?
**Катон** - название планеты, вокруг которой должны были происходить события игры. Помимо разработки компьютерной игры, я занимаюсь созданием и проработкой своей вселенной, лор которой смогу использовать в последующих обновлениях или будущих проектах, посвящённых той же тематике, что и *Katon: Battlespace*.

## Сводка по папке скриптов
[Scripts/](/Katon%20Battlespace/Assets/Scripts/):
- [Modules/](/Katon%20Battlespace/Assets/Scripts/Modules) - скрипты, связанные с созданием, нахождением и функционированием корабельных модулей;
  - [Creation/](/Katon%20Battlespace/Assets/Scripts/Modules/Creation) - раздел, содержащий скрипты для создания модулей;
  - [Data/](/Katon%20Battlespace/Assets/Scripts/Modules/Data) - каталог, содержащий интерфейсы и классы для хранения промежуточных данных о создаваемых модулях;
  - [Ship Modules/](/Katon%20Battlespace/Assets/Scripts/Modules/Ship%20Modules) - раздел, реализующий функционал каждого из модулей;
- [Ship Components/](/Katon%20Battlespace/Assets/Scripts/Ship%20Components) - компоненты и скрипты для космических кораблей;
- [Touchscreen/](/Katon%20Battlespace/Assets/Scripts/Touchscreen) - кастомные компоненты для работы с экранами мобильных устройств;
- [Voxels/](/Katon%20Battlespace/Assets/Scripts/Voxels) - раздел, содержащий скрипты и компоненты, связанные с вокселями;
  - [Data/](/Katon%20Battlespace/Assets/Scripts/Voxels/Data) - каталог, хранящий классы для загрузки данных о вокселях в память устройства;
  - [Editor/](/Katon%20Battlespace/Assets/Scripts/Voxels/Editor) - подраздел, содержащий скрипты и компоненты, связанные с работой вокселей в редакторе кораблей;
    - [Data/](/Katon%20Battlespace/Assets/Scripts/Voxels/Editor/Data) - каталог, хранящий классы для загрузки данных о работе в редакторе в память устройства;

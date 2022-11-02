# RevitSpacesManager_CSharp-WPF
Плагин для ПО Autodesk Revit, предназначенный для обеспечения идентичности 
элементов Spaces(Пространства) в файле инженерных систем и элементов 
Rooms(Помещения) в модели строительной части. Дополнительно предусматривается 
возможность создавать не только Spaces, но и Rooms в текущем открытом файле 
для дисциплины ТХ.

Алгоритм работы плагина:
  - При запуске считываются пространства и помещения из текущей открытой модели 
для дальнейших действий с ними (полного и частичного удаления). Удаление 
осуществляется нажатием кнопок Delete All или Delete Selected.

  - Считываются подгруженные связанные модели и количество помещений в них 
для дальнейшего создания аналогичных пространств или помещений в текущей 
модели (полного и частичного создания). Создание осуществляется нажатием 
кнопок Create All или Create Selected для конкретного линка или конкретной 
фазы выбранного линка.

  - Перед созданием пространств и помещений производится проверка на наличие в 
модели рабочего набора 'Model Spaces' или 'Model Rooms'.

  - Перед созданием пространств и помещений производится проверка на 
корректность размещения помещений в выбранном линке, на наличие совпадающих 
по имени и отметке уровней, содержащих помещения, в линке и текущей модели. 
Помещения, не прошедшие проверку, не создаются, выводятся в информационном 
окне подтверждения создания новых пространств или помещений с рекомендациями 
по устранению ошибок.

  - При создании новых пространств и помещений производится перенос данных об 
уровне, координатах расположения, верхней и нижней границе из модели линка. 
Созданные пространства и помещения автоматически попадают в рабочие наборы 
'Model Spaces' и 'Model Rooms'.
            

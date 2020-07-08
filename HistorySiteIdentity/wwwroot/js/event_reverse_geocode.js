// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.








ymaps.ready(init);

function init() {
    var myPlacemark,x,y,
        myMap = new ymaps.Map('map', {
            center: [55.753994, 37.622093],
            zoom: 9
        }, {
            searchControlProvider: 'yandex#search'
        });

    //Вывод метки по имеющимся координатам

    
        // Слушаем клик на карте.
        myMap.events.add('click', function (e) {

            var coords = e.get('coords');

            // Если метка уже создана – просто передвигаем ее.
            if (myPlacemark) {
                myPlacemark.geometry.setCoordinates(coords);
                //document.getElementById('geomapXY').value = coords;
                
            }
            // Если нет – создаем.
            else {
                myPlacemark = createPlacemark(coords);
                myMap.geoObjects.add(myPlacemark);
                // Слушаем событие окончания перетаскивания на метке.
                myPlacemark.events.add('dragend', function () {
                    getAddress(myPlacemark.geometry.getCoordinates());
                    //document.getElementById('geomapXY').value = coords;
                    
                });
            }
            getAddress(coords);
            //document.getElementById('geomapXY').value = coords;
        });
    var x = Number(document.getElementById('geomapX').textContent);  // textContent!!!!!!!!!!!!!!!!!!!!!!
    var y = Number(document.getElementById('geomapY').textContent);  // видит значения элемента в документе
    var Name = document.getElementById('Name').textContent;
    var adress = document.getElementById('Adress1').textContent;
    //var week = document.getElementById('Week').textContent;
    if (x != 0 && y != 0) {
        myMap.panTo([x,y]);
        var myPlacemark2 = new ymaps.GeoObject({
            geometry: {
                type: "Point",
                coordinates: [x, y]
            }
            ,
            properties: {
                // Контент метки.
                balloonContentHeader: Name,
                balloonContentBody: adress
                //balloonContentFooter: week+"-я неделя войны."
                //iconContent: Name
                
            }
        });
        myMap.geoObjects.add(myPlacemark2);
        //getAddress([myPlacemark2.geometry.getCoordinates()]);
        //getAddress();

    }
    // Создание метки.
    function createPlacemark(coords) {
        return new ymaps.Placemark(coords, {
            iconCaption: 'поиск...'
        }, {
            preset: 'islands#violetDotIconWithCaption',
            draggable: true
        });
    }

    // Определяем адрес по координатам (обратное геокодирование).
    function getAddress(coords) {
        myPlacemark.properties.set('iconCaption', 'поиск...');
        ymaps.geocode(coords).then(function (res) {
            var firstGeoObject = res.geoObjects.get(0);

            myPlacemark.properties
                .set({
                    // Формируем строку с данными об объекте.
                    iconCaption: [
                        // Название населенного пункта или вышестоящее административно-территориальное образование.
                        firstGeoObject.getLocalities().length ? firstGeoObject.getLocalities() : firstGeoObject.getAdministrativeAreas(),
                        // Получаем путь до топонима, если метод вернул null, запрашиваем наименование здания.
                        firstGeoObject.getThoroughfare() || firstGeoObject.getPremise()
                        
                        
                        
                    ].filter(Boolean).join(', '),
                    // В качестве контента балуна задаем строку с адресом объекта.
                    balloonContentBody: firstGeoObject.getAddressLine(),
                    balloonContentFooter: firstGeoObject.geometry.getCoordinates()
                });
            var ad = firstGeoObject.getAddressLine();
            document.getElementById('Adress').value = ad;
            document.getElementById('geomapXY').value = coords;
           

        });
    }
}

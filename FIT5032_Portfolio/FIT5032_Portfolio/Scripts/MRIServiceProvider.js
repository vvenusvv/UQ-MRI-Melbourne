const TOKEN = "pk.eyJ1IjoidnZlbnVzdyIsImEiOiJjbG03OTVpbnY0NjJpM2ZwaDQ1bWhlaXQ1In0.USMwRCTwkywVCJNtOw7eGA";
var locations = [];

$(".coordinates").each(function () {
    var longitude = $(this).find(".Long").text();
    var latitude = $(this).find(".Lat").text();
    var name = $(this).find(".Name").text();

    var point = {
        "latitude": latitude,
        "longitude": longitude,
        "name": name
    };
    locations.push(point);
});

var data = [];
for (i = 0; i < locations.length; i++) {
    var feature = {
        "type": "Feature",
        "properties": {
            "name": locations[i].name
        },
        "geometry": {
            "type": "Point",
            "coordinates": [locations[i].longitude, locations[i].latitude]
        }
    };
    data.push(feature)
}

mapboxgl.accessToken = TOKEN;

var map = new mapboxgl.Map({
    container: 'map',
    style: 'mapbox://styles/mapbox/streets-v12',
    zoom: 11,
    center: [locations[0].longitude, locations[0].latitude]
});

map.on('load', () => {
    map.addSource('providers', {
        type: 'geojson',
        data: {
            "type": "FeatureCollection",
            "features": data
        }
    });

    map.addLayer({
        "id": "places",
        "type": "circle",
        "source": "providers",
        'layout': {
            'visibility': 'visible'
        },
        'paint': {
            'circle-radius': 8,
            'circle-color': '#6c584c'
        }
    });

    map.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            mapboxgl: mapboxgl
        })
    );

    map.addControl(new mapboxgl.NavigationControl());

    map.on('click', 'places', (e) => {
        var coordinates = e.features[0].geometry.coordinates.slice();
        var name = e.features[0].properties.name;

        while (Math.abs(e.lngLat.lng - coordinates[0]) > 180) {
            coordinates[0] += e.lngLat.lng > coordinates[0] ? 360 : -360;
        }
        new mapboxgl.Popup()
            .setLngLat(coordinates)
            .setHTML(name)
            .addTo(map);
    });

    map.on('mouseenter', 'places', (e) => {
        map.getCanvas().style.cursor = 'pointer';
    });

    map.on('mouseleave', 'places', (e) => {
        map.getCanvas().style.cursor = '';
    });
});
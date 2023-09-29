$(document).ready(function () {
    var appointment = [];

    $(".events").each(function () {
        var title = $(this).find(".title").text().trim();

        var date = $(this).find(".date").text();
        var time = $(this).find(".time").text();
        var datetime = date + ' ' + time;

        var d = new Date(datetime);
        var start = moment(d).format();

        var event = {
            "title": title,
            "start": start
        };
        appointment.push(event);
    });

    var today = new Date();
    var d = today.getDate();
    var m = today.getMonth();
    var y = today.getFullYear();

    $('#calendar').fullCalendar({
        editable: true,
        firstDay: 1,
        year: y,
        month: m,
        date: d,
        header: {
            left: 'title',
            right: 'prev, today, next'
        },
        initialView: 'dayGridMonth',
        handleWindowResize: true,
        events: appointment
    });
});

﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.
function setupLastMinute() {
    getLastMinute(function (response) {
        var labelFormat = 'HH:mm:ss';
        var labels = [];
        var tempData = [];
        var smokeData = [];
        var predictionData = [];

        var a = moment().add(-60, 'seconds');
        var b = moment();

        for (var m = moment(a); m.isBefore(b); m.add(1, 'seconds')) {
            labels.push(m.format(labelFormat));
            var found = false;

            response.forEach(function (record) {
                if ((new moment(record.timestamp.value, 'YYYYMMDDHHmmss').format("YYYYMMDDHHmmss")) === m.format("YYYYMMDDHHmmss")) {
                    if (!found) {
                        found = true;
                        tempData.push(record.temperature_value.value);
                        smokeData.push(record.smoke_value.value);
                        predictionData.push(record.risk_level.value);
                        break;
                    }
                }
            });

            if (!found) {
                tempData.push(0);
                smokeData.push(0);
                predictionData.push(0);
            }
        }
        var tempChart = setupTempChart($('#temp1'), labels, tempData);
        var smokeChart = setupSmokeChart($('#smoke1'), labels, smokeData);
        var preChart = setupPredictionChart($('#prediction1'), labels, predictionData);

        setupPolling(1, tempChart, smokeChart, preChart, labelFormat);
    });
}

function setupLastHour() {
    getLastHour(function (response) {
        var labelFormat = 'HH:mm';
        var labels = [];
        var tempData = [];
        var smokeData = [];
        var predictionData = [];

        var a = moment().add(-60, 'minutes');
        var b = moment();

        for (var m = moment(a); m.isBefore(b); m.add(1, 'minutes')) {
            labels.push(m.format(labelFormat));
            var found = false;
            response.forEach(function (record) {
                if ((new moment(record.timestamp.value, 'YYYYMMDDHHmmss').format("YYYYMMDDHHmm")) === m.format("YYYYMMDDHHmm")) {
                    if (!found) {
                        found = true;
                        data.push(record.temperature_value.value);
                        smokeData.push(record.smoke_value.value);
                        predictionData.push(record.risk_level.value);
                    }
                }
            });

            if (!found) {
                tempData.push(null);
                smokeData.push(null);
                predictionData.push(null);
            }
        }

        var tempChart = setupTempChart($('#temp2'), labels, tempData);
        var smokeChart = setupSmokeChart($('#smoke2'), labels, smokeData);
        var preChart = setupPredictionChart($('#prediction2'), labels, predictionData);

        setupPolling(60, tempChart, smokeChart, preChart, labelFormat);


    });
}

function setupLastDay() {
    getLastDay(function (response) {
        var labelFormat = 'HH';
        var labels = [];
        var tempData = [];
        var smokeData = [];
        var predictionData = [];
        var a = moment().add(-24, 'hours');
        var b = moment();

        for (var m = moment(a); m.isBefore(b); m.add(1, 'hours')) {
            labels.push(m.format(labelFormat) + ":00");
            var found = false;
            response.forEach(function (record) {
                if ((new moment(record.timestamp.value, 'YYYYMMDDHHmmss').format("YYYYMMDDHH")) === m.format("YYYYMMDDHH")) {
                    if (!found) {
                        found = true;
                        data.push(record.temperature_value.value);
                        smokeData.push(record.smoke_value.value);
                        predictionData.push(record.risk_level.value);
                    }
                }
            });

            if (!found) {
                tempData.push(null);
                smokeData.push(null);
                predictionData.push(null);
            }
        }
        var tempChart = setupTempChart($('#temp3'), labels, tempData);
        var smokeChart = setupSmokeChart($('#smoke3'), labels, smokeData);
        var preChart = setupPredictionChart($('#prediction3'), labels, predictionData);

        setupPolling(3600, tempChart, smokeChart, preChart, labelFormat);
    });
}

function setupLastWeek() {
    getLastWeek(function (response) {
        var labelFormat = 'DD MMM';
        var labels = [];
        var tempData = [];
        var smokeData = [];
        var predictionData = [];

        var a = moment().add(-7, 'days');
        var b = moment();

        for (var m = moment(a); m.isBefore(b); m.add(1, 'days')) {
            labels.push(m.format(labelFormat));
            var found = false;
            response.forEach(function (record) {
                if ((new moment(record.timestamp.value, 'YYYYMMDDHHmmss').format("YYYYMMDD")) === m.format("YYYYMMDD")) {
                    if (!found) {
                        found = true;
                        data.push(record.temperature_value.value);
                        smokeData.push(record.smoke_value.value);
                        predictionData.push(record.risk_level.value);
                    }
                }
            });

            if (!found) {
                tempData.push(null);
                smokeData.push(null);
                predictionData.push(null);
            }
        }
        var tempChart = setupTempChart($('#temp4'), labels, tempData);
        var smokeChart = setupSmokeChart($('#smoke4'), labels, smokeData);
        var preChart = setupPredictionChart($('#prediction4'), labels, predictionData);

        setupPolling(86400, tempChart, smokeChart, preChart, labelFormat);
    });
}


function setupPolling(interval, tempChart, smokeChart, preChart, labelFormat) {
    setInterval(function () {
        $.ajax({
            url: "/api/data",
            cache: false,
            success: function (response) {

                tempChart.data.datasets.forEach((dataset) => {
                    dataset.data.push(response.temperature_value.value);
                });

                smokeChart.data.datasets.forEach((dataset) => {
                    dataset.data.push(response.smoke_value.value);
                });

                preChart.data.datasets.forEach((dataset) => {
                    dataset.data.push(response.risk_level.value);
                });

                $('#temp').html(response.temperature_value.value);
                $('#smoke').html(response.smoke_value.value);
                $('#prediction').html(response.risk_level.value);
                $('#predictionBig').html(response.risk_level.value);
                $('#lastUpdated').html((new moment(response.timestamp.value, 'YYYYMMDDHHmmss').format("DD/MM/YYYY HH:mm:ss")));
            },
            fail: function (response) {
                tempChart.data.datasets.forEach((dataset) => {
                    dataset.data.push();
                });
                smokeChart.data.datasets.forEach((dataset) => {
                    dataset.data.push();
                });
                preChart.data.datasets.forEach((dataset) => {
                    dataset.data.push();
                });
            },
            complete: function (response) {
                tempChart.data.labels.shift();
                tempChart.data.datasets.forEach((dataset) => {
                    dataset.data.shift();
                });
                tempChart.data.labels.push((moment().format(labelFormat)));
                tempChart.update();


                smokeChart.data.labels.shift();
                smokeChart.data.datasets.forEach((dataset) => {
                    dataset.data.shift();
                });
                smokeChart.data.labels.push((moment().format(labelFormat)));
                smokeChart.update();


                preChart.data.labels.shift();
                preChart.data.datasets.forEach((dataset) => {
                    dataset.data.shift();
                });
                preChart.data.labels.push((moment().format(labelFormat)));
                preChart.update();
            }
        });
    }, interval * 1000);
}

function setupPredictionChart(ctx, labels, data) {
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1,
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 10
                    }
                }]
            },
            legend: {
                display: false,
            }
        }
    });
    return chart;
}

function setupSmokeChart(ctx, labels, data) {
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1,
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: 0,
                        suggestedMax: 10
                    }
                }]
            },
            legend: {
                display: false,
            }
        }
    });
    return chart;
}

function setupTempChart(ctx, labels, data) {
    var chart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: 'rgba(54, 162, 235, 0.2)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 1,
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        suggestedMin: 10,
                        suggestedMax: 30
                    }
                }]
            },
            legend: {
                display: false,
            }
        }
    });
    return chart;
}

function getLastData(callBackFunction) {
    $.ajax({
        url: "/api/data",
        cache: false,
        success: function (response) {
            callBackFunction(response);
        }
    });
}

function getLastMinute(callBackFunction) {
    $.ajax({
        url: "/api/data/lastminute",
        cache: false,
        success: function (response) {
            callBackFunction(response);
        }
    });
}

function getLastHour(callBackFunction) {
    $.ajax({
        url: "/api/data/lasthour",
        cache: false,
        success: function (response) {
            callBackFunction(response);
        }
    });
}

function getLastDay(callBackFunction) {
    $.ajax({
        url: "/api/data/lastday",
        cache: false,
        success: function (response) {
            callBackFunction(response);
        }
    });
}

function getLastWeek(callBackFunction) {
    $.ajax({
        url: "/api/data/lastweek",
        cache: false,
        success: function (response) {
            callBackFunction(response);
        }
    });
}
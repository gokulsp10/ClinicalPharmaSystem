var query = '';
var hours = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
var hours24 = ['13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24'];
var hoursUpdated = ['01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12'];
var minutes = ['00', '30'];
var format = ['am', 'pm'];
var time;
var timeArray = [];
var items = [];
var error;
var errorMessage;
var list = $('#list');

itemList(this.hours, this.minutes, this.format, this.time, this.timeArray);

function itemList(hours, minutes, format, time, timeArray) {
    hours.map(i => {
        minutes.map(j => {
            format.map(k => {
                time = i + ":" + j + " " + k;
                timeArray.push(time);
                this.timeArray = timeArray;
            });
        });
    });
}

function querySubset(query) {
    this.query = query.toLowerCase();
    this.hour = query.substring(0, 2);
    this.timeSelector = query.substring(2, 3);
    this.minute = query.substring(3, 5);
    this.formatSelector = query.substring(5, 6);
    this.formatUpdated = query.substring(6, 8);
}

function hoursUpdate(input) {
    this.hours24.map((value, i) => {
        if (input === value) {
            this.query = this.hoursUpdated[i];
            $("#hour-input").val(this.query);
        }
    });
}

function bindResult() {
    list.show();
    this.error = false;
    $('#hour-input').removeClass('active');
    $('.error').html('').removeClass("alert");

    this.items = this.timeArray.filter(function (jam) {
        return jam.indexOf(this.query) > -1;
    }.bind(this));

    list.empty();
    this.items.forEach(function (hour, i) {
        var entry = document.createElement('li');
        entry.append(document.createTextNode(hour));
        entry.id = i;
        entry.setAttribute("onclick", "select('" + hour + "')");
        list.append(entry);
    });

}

function onErrorResult(errorMes) {
    this.error = true;
    this.errorMessage = errorMes;
    $('.time-picker').attr('data-original-title', this.errorMessage);
    $('.error').html(this.errorMessage).addClass(' alert alert-danger');
    $('#hour-input').addClass('active');
}

function filter(query) {
    if (query !== "") {
        this.querySubset(query);
        if (!isNaN(this.hour)) {
            this.hoursUpdate(this.hour);
            this.bindResult();
            if (this.timeSelector === ":") {
                this.bindResult();
                if (this.minute !== "") {
                    if (!isNaN(this.minute))
                        this.bindResult();
                    if (this.formatSelector === " ") {
                        this.bindResult();
                        if (this.formatUpdated === "am" || this.formatUpdated === "pm") {
                            this.bindResult();
                        } else {
                            errorMessage = "Define am/pm";
                            this.onErrorResult(errorMessage);
                        }
                    } else {
                        errorMessage = "Use space before am/pm";
                        this.onErrorResult(errorMessage);
                    }
                } else {
                    errorMessage = "Fill your minute with number";
                    this.onErrorResult(errorMessage);
                }
            } else {
                errorMessage = "Use ' : ' between hour and minute";
                this.onErrorResult(errorMessage);
            }
        } else {
            errorMessage = "Fill your hour with number";
            this.onErrorResult(errorMessage);
        }
    } else {
        this.error = false;
        this.errorMessage = "";
        $('.error').html('').removeClass("alert");
        $('#hour-input').removeClass('active');
        this.items = [];
    }
}

function select(item) {
    list.hide();
    this.error = false;
    this.errorMessage = "";
    this.query = item;
    $("#hour-input").val(this.query);
    $('#hour-input').removeClass('active');
    $('.error').html('').removeClass("alert");
    this.items = [];
}
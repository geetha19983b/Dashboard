var towers, summaryPeriods, dashboardData, towersSelected, prioritySelected, metalSelected;

var currentBacklogData, currentAgingData;
function listadd(chkboxitem) {


    var $list = $("#itemList");
    var a = $(chkboxitem).val();
    var $chk = $(chkboxitem);
    
    //if ($(chkboxitem).checked)
    if($chk.is(':checked'))
    {
        
        $list.append('<li class="lgi btn btn-sm btn-primary"><a href="#" class="filterref"><span class=label label-info>' + a + '</span></a><button class="closebutton btn btn-xs btn-warning" value="' + a + '"><span class="glyphicon glyphicon-remove"></span></button></li>');

    }
    else {
        $("#itemList li:contains('" + a + "')").remove();
    }





}
function renderDashboardCharts() {
    //alert("1");
    $("#inflow-chart").empty();
    $("#resolution-rate").empty();
    $("#resolution-sla").empty();
    $("#response-sla").empty();
    $("#priority-chart").empty();
    $("#backlog-chart").empty();

    //alert("2");
    $('#sel_filter_options').empty();

    // populate filter months based on period drop down selected
    var selectPeriod = parseInt($("#selectPeriod").val());
    var periodSelection = [];
    var today = new Date();

    for (var periodCounter = 0; periodCounter < selectPeriod; periodCounter++) {

        var newdate = new Date(today);
        newdate.setMonth(newdate.getMonth() - periodCounter);
        var nd = new Date(newdate);

        periodSelection.push(nd.getFullYear() + '-' + (nd.getMonth() + 1));
    }

    //var periodSelection = [];
    //$('#selectmultiple option:selected').each(function () {
    //    periodSelection.push(summaryPeriods[$(this).val()].value);
    //});

    var towerSelection = [];

    $.each($("input[name='Towers']:checked"), function () {
        towerSelection.push($(this).val());
        $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");

    });

    //alert("3");

    var prioritySelection = [];

    $.each($("input[name='Priorities']:checked"), function () {
        prioritySelection.push($(this).val());
        $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");

    });

    var metalSelection = [];

    $.each($("input[name='MetalSelection']:checked"), function () {
        metalSelection.push($(this).val());
        $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");

    });

    //alert("4");

    var filteredDashboardData = [];
    var selectedPeriodCount = periodSelection.length;
    var selectedPeriodCountTile = periodSelection.length;
    var selectedTowerCount = towerSelection.length;
    var dashboardDataCount = dashboardData.length;
    var selectedPriorityCount = prioritySelection.length;
    var selectedMetalCount = metalSelection.length;

    //alert("5");

    //if (selectedPeriodCount == 0) {
    //    $('#selectmultiple option').prop('selected', true);

    //    $('#selectmultiple option:selected').each(function () {
    //        periodSelection.push(summaryPeriods[$(this).val()].value);
    //    });

    //    selectedPeriodCount = periodSelection.length;
    //}

    ////alert("6");

    //if (selectedPeriodCount == 1) {
    //    var selectedPeriod = $('#selectmultiple option:selected').val();
    //    $('#selectmultiple option').each(function (i) {
    //        if (i < selectedPeriod) {
    //            periodSelection.push(summaryPeriods[$(this).val()].value);
    //        }
    //    });

    //    selectedPeriodCount = periodSelection.length;
    //}

    //alert("7");

    if (selectedTowerCount == 0) {
        //towerSelection = towersSelected.split(",");


        $("input[name='Towers']").prop('checked', true);

       
        //var chkbxname = "\"input[name='Towers']\"";
        //listadd(chkbxname);
        $.each($("input[name='Towers']:checked"), function () {
            $("#checkAllTowers").prop('checked', true);

            //listadd($(this));

            towerSelection.push($(this).val());
            $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");
        });
        towersSelected = towerSelection;
        selectedTowerCount = towerSelection.length;
    }

    //alert("8");

    if (selectedPriorityCount == 0) {

        $("input[name='Priorities']").prop('checked', true);

        $.each($("input[name='Priorities']:checked"), function () {
            $("#checkAllPriorities").prop('checked', true);
            //listadd($(this));
            prioritySelection.push($(this).val());
            $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");
        });

        selectedPriorityCount = prioritySelection.length;
    }

    //alert("9");

    if (selectedMetalCount == 0) {

        $("input[name='MetalSelection']").prop('checked', true);

        $.each($("input[name='MetalSelection']:checked"), function () {
            $("#checkAllMetal").prop('checked', true);
            //listadd($(this));
            metalSelection.push($(this).val());
            $('#sel_filter_options').append("<span class='tag label label-info'>" + $(this).val() + "<span class='filter_remove' filter_attr='" + $(this).attr("id") + "' data-role='remove'></span>");
        });

        selectedMetalCount = metalSelection.length;
    }

    //alert("10");

    //var towers = towerSelection.join();
    //var period = $(periodSelection).last()[0];

    //$("input[id=ContentPlaceHolder1_towerSel]").val(towers);

    //$("input[id=ContentPlaceHolder1_periodSel]").val(period);


    // Filter DB Data to user preferred data
    for (var dashboardDataCounter = 0; dashboardDataCounter < dashboardDataCount; dashboardDataCounter++) {
        //alert("01");
        for (var selectedPeriodCounter = 0; selectedPeriodCounter < selectedPeriodCount; selectedPeriodCounter++) {
            //alert("02");
            //alert(dashboardData[dashboardDataCounter].period + "--" + periodSelection[selectedPeriodCounter] + " ++ " + (dashboardData[dashboardDataCounter].period == periodSelection[selectedPeriodCounter]));
            if (dashboardData[dashboardDataCounter].period == periodSelection[selectedPeriodCounter]) {
                //alert("03");
                var periodData = { 'period': dashboardData[dashboardDataCounter].period };
                var values = [];
                //alert("04");
                for (var selectedTowerCounter = 0; selectedTowerCounter < selectedTowerCount; selectedTowerCounter++) {
                    //alert("05");
                    for (var selectedPriorityCounter = 0; selectedPriorityCounter < selectedPriorityCount; selectedPriorityCounter++) {
                        //alert("06");
                        for (var selectedMetalCounter = 0; selectedMetalCounter < selectedMetalCount; selectedMetalCounter++) {
                            //alert("07");
                            var valueCount = dashboardData[dashboardDataCounter].values.length;
                            for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
                                //alert("08");
                                //alert(dashboardData[dashboardDataCounter].values[valueCounter].Tower + ")) " + towerSelection[selectedTowerCounter] + " ;; " + (dashboardData[dashboardDataCounter].values[valueCounter].Tower == towerSelection[selectedTowerCounter]));
                                if (dashboardData[dashboardDataCounter].values[valueCounter].Tower == towerSelection[selectedTowerCounter] && dashboardData[dashboardDataCounter].values[valueCounter].Metal == metalSelection[selectedMetalCounter] && dashboardData[dashboardDataCounter].values[valueCounter].IncidentPriority == prioritySelection[selectedPriorityCounter]) {
                                    //alert("09");
                                    values.push(dashboardData[dashboardDataCounter].values[valueCounter]);
                                }
                            }
                        }
                    }
                }

                periodData.values = values;
                filteredDashboardData.push(periodData);
            }
        }
    }

    //alert("11");


    var resolutionRateData = [];
    var resolutionSLAData = [];
    var responseSLAData = [];
    var inflowData = [];
    var priorityData = [];
    var backlogData = [];

    var resolvedData = [];
    var inventoryData = [];

    var filteredDashboardDataCount = filteredDashboardData.length;

    if (filteredDashboardDataCount == 0) {
        filteredDashboardData = dashboardData;
        filteredDashboardDataCount = dashboardDataCount;
    }

    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }

            periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;

            //if (selectDataType == "1") {
            //    periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
            //}
            //else {
            //    periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
            //}
        }
        inventoryData.push(periodData);
    }

    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }

            periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;

        }
        resolvedData.push(periodData);
    }

    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }

            if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {

                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;

                //if (selectDataType == "1") {
                //    periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
                //}
                //else {
                //    periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
                //}
            }
        }
        inflowData.push(periodData);
    }

    // Resolution Rate Data -- resolutionRateData
    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }

            if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {

                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = parseInt(periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower]) + parseInt(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved);
            }
        }
        resolutionRateData.push(periodData);
    }
    // Resolution Rate Data Filter-- resolutionRateData
    for (var resolutionRateDataCounter = 0; resolutionRateDataCounter < resolutionRateData.length; resolutionRateDataCounter++) {
        for (var selectedTowerCounter = 0; selectedTowerCounter < selectedTowerCount; selectedTowerCounter++) {

            resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = parseInt((resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] * 100) / inventoryData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]]);

            if (resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == null || resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == undefined || resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "null" || resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "" || isNaN(resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]])) {
                resolutionRateData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = 0;
            }
        }
    }

    // Resolution SLA Data -- resolutionSLAData
    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }

            if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                   $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                   $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {

                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = parseInt(periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower]) + parseInt(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].ResolutionSLA);
            }
        }
        resolutionSLAData.push(periodData);
    }

    // Resolution SLA Data Filter-- resolutionSLAData
    for (var resolutionRateDataCounter = 0; resolutionRateDataCounter < resolutionSLAData.length; resolutionRateDataCounter++) {
        for (var selectedTowerCounter = 0; selectedTowerCounter < selectedTowerCount; selectedTowerCounter++) {

            resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = parseInt((resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] * 100) / inventoryData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]]);

            if (resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == null || resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == undefined || resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "null" || resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "" || isNaN(resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]])) {
                resolutionSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = 0;
            }
        }
    }

    // Response SLA Data -- responseSLAData
    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] == undefined) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = 0;
            }
            if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {
                periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower] = parseInt(periodData[filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower]) + parseInt(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].ResponseSLA);
            }
        }
        responseSLAData.push(periodData);
    }
    // Response SLA Data Filter-- responseSLAData
    for (var resolutionRateDataCounter = 0; resolutionRateDataCounter < responseSLAData.length; resolutionRateDataCounter++) {
        for (var selectedTowerCounter = 0; selectedTowerCounter < selectedTowerCount; selectedTowerCounter++) {

            responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = parseInt((responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] * 100) / inventoryData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]]);

            if (responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == null || responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == undefined || responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "null" || responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] == "" || isNaN(responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]])) {
                responseSLAData[resolutionRateDataCounter][towerSelection[selectedTowerCounter]] = 0;
            }
        }
    }


    // P1/P2 Trend
    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount - 1; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period, "Inflow": 0, "Resolved": 1, "Backlog": 2 };
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority == "1 - Critical" || filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority == "2 - High") {
                if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {
                    periodData["Inflow"] = periodData["Inflow"] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
                    periodData["Resolved"] = periodData["Resolved"] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
                    periodData["Backlog"] = periodData["Backlog"] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
                }
            }
        }
        priorityData.push(periodData);
    }


    // Backlog Trend
    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount - 1; filteredDashboardDataCounter++) {
        var periodData = { "period": filteredDashboardData[filteredDashboardDataCounter].period, "P1-Backlog": 0, "P2-Backlog": 1 };
        
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            if (filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority == "1 - Critical") {
                if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                        $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                        $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {
                    periodData["P1-Backlog"] = periodData["P1-Backlog"] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
                }
            }
            if (filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority == "2 - High") {
                if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 &&
                        $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                        $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {
                    periodData["P2-Backlog"] = periodData["P2-Backlog"] + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
                }
            }
        }
        backlogData.push(periodData);
    }


    // Get Summary Data
    var inflowCount = 0;
    var inventoryCount = 0;
    var rslvdCount = 0;
    var brchdCount = 0;
    var backlogCount = 0;


    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;

        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
            // 2015-11, 2016-03

            var today = new Date();
            // Add 1 in future
            var mm = today.getMonth(); 
            var yyyy = today.getFullYear();

            var period = today.getFullYear() + '-' + today.getMonth();

            if (filteredDashboardData[filteredDashboardDataCounter].period == period) {
                if ($.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Tower, towerSelection) > -1 && 
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].IncidentPriority, prioritySelection) > -1 &&
                    $.inArray(filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Metal, metalSelection) > -1) {
                    inflowCount = inflowCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
                    rslvdCount = rslvdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
                    brchdCount = brchdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Breached;
                    backlogCount = backlogCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
                }
            }
        }
    }

    //if (selectedPeriodCountTile == 1) {
    //    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
    //        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
    //        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
    //            if (filteredDashboardData[filteredDashboardDataCounter].period == summaryPeriods[$('#selectmultiple option:selected').val()].value) {
    //                if (selectDataType == "1") {
    //                    inflowCount = inflowCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //                    rslvdCount = rslvdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
    //                    brchdCount = brchdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Breached;
    //                    //pendCount = pendCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Pen;
    //                    backlogCount = backlogCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //                }
    //                else {
    //                    inflowCount = inflowCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
    //                    rslvdCount = rslvdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
    //                    brchdCount = brchdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Breached;
    //                    backlogCount = backlogCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //                }
    //            }
    //        }
    //    }
    //}
    //else {
    //    for (var filteredDashboardDataCounter = 0; filteredDashboardDataCounter < filteredDashboardDataCount; filteredDashboardDataCounter++) {
    //        var valueCount = filteredDashboardData[filteredDashboardDataCounter].values.length;
    //        for (var valueCounter = 0; valueCounter < valueCount; valueCounter++) {
    //            if (selectDataType == "1") {
    //                inflowCount = inflowCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //                rslvdCount = rslvdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
    //                brchdCount = brchdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Breached;
    //                backlogCount = backlogCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //            }
    //            else {
    //                inflowCount = inflowCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Inflow;
    //                rslvdCount = rslvdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Resolved;
    //                brchdCount = brchdCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Breached;
    //                backlogCount = backlogCount + filteredDashboardData[filteredDashboardDataCounter].values[valueCounter].Backlog;
    //            }
    //        }
    //    }
    //}
   // inflowData=  eval(Inflowdata());
   // $("#inflow-chart").empty();
    //var statearrwhr = ["JAN", "FEB", "Mar", "Apr"];

    Morris.Bar({
        element: 'inflow-chart',
        data: inflowData,
        xkey: 'period',
        ykeys: towerSelection,
        labels: towerSelection,
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        xLabelFormat: getXLabels
    })

    .on('click', function (i, row, x, y, sidx) {
        var url = $('#bootstrapDialog').data('url');
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
        });
    });

    ;

    Morris.Bar({
        element: 'resolution-rate',
        data: resolutionRateData,
        xkey: 'period',
        ykeys: towerSelection,
        labels: towerSelection,
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        xLabelFormat: getXLabels
    });

    Morris.Bar({
        element: 'resolution-sla',
        data: resolutionSLAData,
        xkey: 'period',
        ykeys: towerSelection,
        labels: towerSelection,
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        xLabelFormat: getXLabels
    })


    Morris.Bar({
        element: 'response-sla',
        data: responseSLAData,
        xkey: 'period',
        ykeys: towerSelection,
        labels: towerSelection,
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        xLabelFormat: getXLabels
    });

    Morris.Line({
        element: 'priority-chart',
        data: priorityData,
        xkey: 'period',
        ykeys: ["Inflow", "Resolved", "Backlog"],
        labels: ["Inflow", "Resolved", "Backlog"],
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        yLabelFormat: function (y) { return parseInt(y); },
        xLabelFormat: function (x) {
            var mon = x.toString().split(" ");
            var res = mon[3].substring(2, 4);
            return mon[1] + "-" + res;
        }
    });

    Morris.Line({
        element: 'backlog-chart',
        data: backlogData,
        xkey: 'period',
        ykeys: ["P1-Backlog", "P2-Backlog"],
        labels: ["P1-Backlog", "P2-Backlog"],
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"],
        xLabelFormat: function (x) {
            var mon = x.toString().split(" ");
            var res = mon[3].substring(2, 4);
            return mon[1] + "-" + res;
        }



    });

    $("#TotalIncCnt").text(inflowCount);
    $("#ResIncCnt").text(rslvdCount);
    $("#PendIncCnt").text(backlogCount);
    $("#BrcIncCnt").text(brchdCount);

    //  Backlog chart
    //currentBacklogData = [{ "Tower": "Core", "PendingChange": 1, "PendingUser": 59, "PendingValidation": 38, "PendingVendor": 20, "WorkInProgress": 73 },{ "Tower": "Corporate", "PendingChange": 0, "PendingUser": 3, "PendingValidation": 1, "PendingVendor": 1, "WorkInProgress": 0 }, { "Tower": "CI", "PendingChange": 3, "PendingUser": 9, "PendingValidation": 22, "PendingVendor": 56, "WorkInProgress": 1 }, { "Tower": "IM", "PendingChange": 2, "PendingUser": 12, "PendingValidation": 11, "PendingVendor": 3, "WorkInProgress": 12 }, { "Tower": "TPI", "PendingChange": 2, "PendingUser": 10, "PendingValidation": 5, "PendingVendor": 2, "WorkInProgress": 13 }];
    currentBacklogData = eval(BackLogBarData());
    //alert(currentBacklogData);

    //currentBacklogData = [{ "Tower": "Core", "PendingChange": 55, "PendingUser": 59, "PendingValidation": 38, "PendingVendor": 20, "WorkInProgress": 73 }, { "Tower": "Corporate", "PendingChange": 0, "PendingUser": 3, "PendingValidation": 1, "PendingVendor": 1, "WorkInProgress": 9 }, { "Tower": "CI", "PendingChange": 21, "PendingUser": 9, "PendingValidation": 22, "PendingVendor": 56, "WorkInProgress": 1 }, { "Tower": "IM", "PendingChange": 16, "PendingUser": 12, "PendingValidation": 11, "PendingVendor": 3, "WorkInProgress": 12 }, { "Tower": "TPI", "PendingChange": 2, "PendingUser": 10, "PendingValidation": 5, "PendingVendor": 2, "WorkInProgress": 13 }];
    $("#backlog_inc").empty();
    var statearr = ["PendingChange", "PendingUser", "PendingValidation", "PendingVendor", "WorkInProgress"];
    var statearrwhr = ["Pending Change", "Pending User/Appointment", "Pending Validation", "Pending Vendor", "Work in Progress"];

    Morris.Bar({
        element: 'backlog_inc',
        data: currentBacklogData,
        xkey: 'Tower',
        //ykeys: ["PendingChange", "PendingUser", "PendingValidation", "PendingVendor", "WorkInProgress"],
        ykeys:statearr,
        labels: ["Change", "User", "Validation", "Vendor", "WIP"],
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F"]
    })
        .on('click', function (i, row, x, y, sidx) {
        //alert(currentBacklogData[i].Tower + "-" + statearrwhr[sidx]);

        var url = $('#bootstrapDialog').data('url') + '?chart=Backlog&Tower=' + currentBacklogData[i].Tower + "&Aging=" + statearrwhr[sidx];
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
            $('#bootstrapDialog').modal('show');
        });
    });



    // Aging Chart
    //currentAgingData = [{ "Tower": "Core", "AgingDays3": 15, "AgingDaysgreaterthan3": 22, "AgingDaysgreaterthan7": 21, "AgingDaysgreaterthan10": 187 }, { "Tower": "Corporate", "AgingDays3": 1, "AgingDaysgreaterthan3": 2, "AgingDaysgreaterthan7": 0, "AgingDaysgreaterthan10": 5 }, { "Tower": "CI", "AgingDays3": 28, "AgingDaysgreaterthan3": 14, "AgingDaysgreaterthan7": 5, "AgingDaysgreaterthan10": 62 }, { "Tower": "IM", "AgingDays3": 4, "AgingDaysgreaterthan3": 5, "AgingDaysgreaterthan7": 3, "AgingDaysgreaterthan10": 42 }, { "Tower": "TPI", "AgingDays3": 9, "AgingDaysgreaterthan3": 4, "AgingDaysgreaterthan7": 2, "AgingDaysgreaterthan10": 17 }];

    currentAgingData = eval(AgingBarData());
    
    $("#aging_inc").empty();
    var agearr = ["AgingDays3", "AgingDaysgreaterthan3", "AgingDaysgreaterthan7", "AgingDaysgreaterthan10"];
    var towerarr = [];
    var agearrwhr = ["<= 3", ">3", ">7", ">10"];
    Morris.Bar({
        element: 'aging_inc',
        data: currentAgingData,
        xkey: 'Tower',
        ykeys:agearr,
        labels: ["<= 3", "> 3", "> 7", "> 10"],
        hideHover: 'auto',
        resize: true,
        barColors: ["#85C1E9", "#FAD7A0", "#D7DBDD", "#F7DC6F", "#82E0AA"]
    }).on('click', function (i, row, x, y, sidx) {
        //alert(currentAgingData[i].Tower + "-" + agearrwhr[sidx]);
         
        var url = $('#bootstrapDialog').data('url') + '?chart=Aging&Tower=' + currentAgingData[i].Tower + "&Aging=" + agearrwhr[sidx];
        $.get(url, function (data) {
            $('#bootstrapDialog').html(data);
             $('#bootstrapDialog').modal('show');
           
        
        });

    });

    $(".filter_remove").click(
       function () {
           $("#" + $(this).attr("filter_attr")).prop('checked', false);
           renderDashboardCharts();
       });
}

$(function () {
    towers = [
    { tower: "Core System", color: '#ffcc66', alias: 'Core' },
    { tower: "Customer Interaction", color: '#ffb366', alias: 'CI' },
    { tower: "Third Party Integration", color: '#ff8533', alias: 'TPI' },
    { tower: "Corporate System", color: '#804000', alias: 'Corp' },
    { tower: "Information Management", color: '#991f00', alias: 'IM' }
    ];

    summaryPeriods = [
    { Year: "2015", Month: "Nov", MM: "11", value: "2015-11" },
    { Year: "2015", Month: "Dec", MM: "12", value: "2015-12" },
    { Year: "2016", Month: "Jan", MM: "1", value: "2016-1" },
    { Year: "2016", Month: "Feb", MM: "2", value: "2016-2" },
    { Year: "2016", Month: "Mar", MM: "3", value: "2016-3" }
    ];

    dashboardData = eval($('#chartsData').val());
    
   
    //towersSelected = $('#ContentPlaceHolder1_towerSel').val();

    renderDashboardCharts();
});

function getXLabels(x) { for (var valueCounter = 0; valueCounter < summaryPeriods.length; valueCounter++) { if (summaryPeriods[valueCounter].value == x.label) { return summaryPeriods[valueCounter].Month + " - " + summaryPeriods[valueCounter].Year; } } return ""; }

function AgingBarData() {
    var agingdata = "";

    var strUrl = "";
    var selectedTowers = towersSelected;

    if (rootDir != "/") {
        strUrl = rootDir + 'Home/AgingData?Towers=' + towersSelected;
    }
    else {
        strUrl = '/Home/AgingData?Towers=' + towersSelected;
    }
    $.ajax({
        type: 'GET',
        url: strUrl,
        dataType: 'json',
        async: false,
        contentType: "application/json; charset=utf-8",
        agingdata: {},
        success: function (result) {
            agingdata = result;
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
   
    return agingdata;
}

function BackLogBarData() {
    var bcklogdata = "";
    var strUrl = "";
   
   
    if (rootDir != "/")
    {
        strUrl = rootDir + 'Home/BackLogData?Towers=' + towersSelected;
    }
    else
    {
        strUrl = '/Home/BackLogData?Towers=' + towersSelected;
    }



    $.ajax({
        type: 'GET',
        url:  strUrl,
        dataType: 'json',
        async: false,
        contentType: "application/json; charset=utf-8",
        bcklogdata: {},
        success: function (result) {
            bcklogdata = result;
        },
        error: function (xhr, status, error) {
            alert(error);
        }
    });
    
    return bcklogdata;
}


//function Inflowdata() {
//    var inflow = "";
//    var strUrl = "";


//    if (rootDir != "/") {
//        strUrl = rootDir + 'Home/BackLogData';
//    }
//    else {
//        strUrl = '/Home/BackLogData';
//    }



//    $.ajax({
//        type: 'GET',
//        url: strUrl,
//        dataType: 'json',
//        async: false,
//        contentType: "application/json; charset=utf-8",
//        inflow: {},
//        success: function (result) {
//            inflow = result;
//        },
//        error: function (xhr, status, error) {
//            alert(error);
//        }
//    });

//    return inflow;
//}



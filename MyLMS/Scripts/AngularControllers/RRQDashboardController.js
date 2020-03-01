myapp.controller('RRQDashboardCntrl', function ($scope, $http) {

    GetRRQID(window.location.href);

    $scope.LabelsArray = [];
    $scope.RRQ_ID;

    var Labels = [];
    var Questions = [];
    var chart;
    $scope.RevealAnswer = false;

    function GetRRQID(url) {
        var params = parseURLParams(url);
        if ("RRQID" in params) {
            $scope.RRQ_ID = params["RRQID"][0];

            $http({
                method: 'POST',
                url: '/SessionMgmt/SetSessionRRQ',
                data: { rrqID: $scope.RRQ_ID }
            })

            GetRRQQuestions();
            //GetTop10Students();
        }
    }

    function GetRRQQuestions() {
        $http.get('/SessionMgmt/GetRRQQuestions/' + $scope.RRQ_ID)
            .then(function (result) {
            GetDegreeOfDifficulty();
            $scope.RRQQuestions = result.data;
            document.getElementById('RRQName').textContent = result.data[0].RRQName;
        });
    }

    // Get RRQ Questions
    $scope.GetRRQQuestionAndOptions = function (QID) {
        $scope.QID = QID;

        $http.get('/QuestionBank/GetRRQQuestionAndOptions/' + QID)
            .then(function (result) {
                $scope.QuestionList = result.data;
                $scope.StudentsListByResp = "";
            });

        GetDashboardData($scope.QID);
        GetDashboardOptionGraph($scope.QID);
        //GetTop10FastestStudents($scope.QID);
    }

    // Get Top 10 Students
    $scope.GetTop10Students = function () {
        $http.get('/SessionMgmt/GetTop10Students/' + $scope.RRQ_ID)
            .then(function (result) {
                var data = result.data;
                for (var i = 0; i < data.length; i++) {
                    d = data[i];
                    data[i].TotalMarks = Math.round((d.TotalMarks / d.TotalRRQMarks) * 10000) / 100;
                }
            $scope.Top10Students = data;
        });

    }

    // Get Top 10 Fastest Correct Students
    function GetTop10FastestStudents(QID) {
        $http.get('/SessionMgmt/GetTop10FastestStudents/' + $scope.QID)
            .then(function (result) {                
                $scope.Top10FastestStudents = result.data;
            });
    }

    function GetDegreeOfDifficulty() {
        
        $http.get('/SessionMgmt/GetDegreeOfDifficulty/' + $scope.RRQ_ID)
            .then(function (result) {
                $scope.GraphData = result.data;
                PrepareGraphData();

                var ctx = document.getElementById("canvas").getContext("2d");
                chart = new Chart(ctx).HorizontalBar(barChartData, {
                    responsive: true,
                    barShowStroke: false
                });
            });
            
    }
    function PrepareGraphData() {
        var q = [];
        var l = [];
        angular.forEach($scope.RRQQuestions, function (val, key) {
            l.push('Q-' + val.QUES_NO.toString());
            var check = false;
            angular.forEach($scope.GraphData, function (value, key) {
                if (value.QUES_NO.toString() == val.QUES_NO.toString()) {
                    check = true;
                    q.push(Math.round((1 - (value.TOTAL_CorrectResp / value.TOTAL_RESP)) * 100));
                }
            });
            if (!check)
               q.push(100);

        });

        var list = []
        for (var j = 0; j < l.length; j++)
            list.push({ 'label': l[j], 'age': q[j] });

        //2) sort:
        list.sort(function (a, b) {
            return ((a.age < b.age) ? 1 : ((a.age == b.age) ? 0 : -1));
        });

        //3) separate them back out:
        for (var k = 0; k < list.length; k++) {
             Labels.push(list[k].label);
             Questions.push(list[k].age);
        }

        /*angular.forEach($scope.GraphData, function (value, key) {
            Labels.push('Q-' + value.QUES_NO.toString());
            Questions.push(value.PERCENTAGE);
        })*/
        //console.log(JSON.stringify(Labels));
        //console.log(JSON.stringify(Questions));
    }

    var randomScalingFactor = function () {
        return Math.round(Math.random() * 100);
    };
    var barChartData = {
        labels: Labels,
        datasets: [
            {
                barThickness: "flex" ,
                fillColor: "rgba(25,186,171,0.85)",
                strokeColor: "rgba(220,220,220,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: Questions                
            }
        ]
    };

    $("canvas").click(
        function (evt) {
            var activeBars = chart.getBarsAtEvent(evt)[0];

            var q = activeBars.label;
            var Ques_num = q.substring(q.indexOf('-') + 1);
            for (var i = 0; i < $scope.RRQQuestions.length; i++) {
                e = $scope.RRQQuestions[i];
                if (e.QUES_NO == Ques_num) {
                    $scope.GetRRQQuestionAndOptions(e.QID);
                    break;
                }
            }

            
        }
    );

    $scope.GetStudentsByResponse = function (OptionSeq) {
        $http.get('/SessionMgmt/GetStudentsByResponse/' + $scope.QID + '?OptionSeq=' + OptionSeq)
            .then(function (result) {
                $scope.StudentsListByResp = result.data;
            });
    }
    $scope.MarkAnswer = function () {
        $scope.RevealAnswer = true;
    }
    ///////// Get Top 10 Students ////////
    function GetDashboardData(QID) {
        $http.get('/SessionMgmt/GetDashboardData/' + QID)
            .then(function (result) {
                $scope.DashboardData = result.data;
                $scope.ResponsesPrcnt = $scope.DashboardData[0].ResponsesPrcnt
                $scope.NoResponsesPrcnt = $scope.DashboardData[0].NoResponsesPrcnt
                $scope.CorrectPrcnt = $scope.DashboardData[0].CorrectPrcnt
                $scope.InCorrestPrcnt = $scope.DashboardData[0].InCorrestPrcnt
                $scope.RevealAnswer = false;
                $scope.CorrectOption = $scope.DashboardData[0].CorrectOption
            });
    }
    function GetDashboardOptionGraph(QID) {
        $http.get('/SessionMgmt/GetDashboardOptionGraph/' + QID)
            .then(function (result) {
                $scope.DashboardGraphData = result.data;
                $scope.OPTION_1_PRCNT = $scope.DashboardGraphData[0].OPTION_1_PRCNT
                $scope.OPTION_2_PRCNT = $scope.DashboardGraphData[0].OPTION_2_PRCNT
                $scope.OPTION_3_PRCNT = $scope.DashboardGraphData[0].OPTION_3_PRCNT
                $scope.OPTION_4_PRCNT = $scope.DashboardGraphData[0].OPTION_4_PRCNT

                /*
                var total = OPTION_1_PRCNT + OPTION_2_PRCNT + OPTION_3_PRCNT + OPTION_4_PRCNT;
                $scope.OPTION_1_PRCNT = Math.round((OPTION_1_PRCNT/total)*100)
                $scope.OPTION_2_PRCNT = Math.round((OPTION_2_PRCNT / total) * 100)
                $scope.OPTION_3_PRCNT = Math.round((OPTION_3_PRCNT / total) * 100)
                $scope.OPTION_4_PRCNT = Math.round((OPTION_4_PRCNT / total) * 100)                
                */

            });
    }

    function parseURLParams(url) {
        var queryStart = url.indexOf("?") + 1,
            queryEnd = url.indexOf("#") + 1 || url.length + 1,
            query = url.slice(queryStart, queryEnd - 1),
            pairs = query.replace(/\+/g, " ").split("&"),
            parms = {}, i, n, v, nv;

        if (query === url || query === "") return;

        for (i = 0; i < pairs.length; i++) {
            nv = pairs[i].split("=", 2);
            n = decodeURIComponent(nv[0]);
            v = decodeURIComponent(nv[1]);

            if (!parms.hasOwnProperty(n)) parms[n] = [];
            parms[n].push(nv.length === 2 ? v : null);
        }
        return parms;
    }
});

myapp.controller('RRQReportCntrl', function ($scope, $http) {
    GetRRQID(window.location.href);
    $scope.RRQTable = []


    function GetRRQID(url) {
        var params = parseURLParams(url);
        if ("RRQID" in params) {
            $scope.RRQ_ID = params["RRQID"][0];
            GenerateReport();
        }
    }

    function GenerateReport() {
        $http.get('/StudentMgmt/GenerateRRQStudentReports/' + $scope.RRQ_ID)
            .then(function (result) {
                console.log(result.data);

                var RRQ = result.data.RRQ;
                var ReportData = result.data.StudentReports;

                var RRQRows = []; var RRQCols = [];                
                for (var i in RRQ) {
                    if (i == 'Questions') {
                        for (var j in RRQ.Questions) {
                            RRQCols.push("Q "+ (parseInt(j) + 1));
                            RRQRows.push(RRQ.Questions[j].OptionChar);
                        }                        
                    }
                    else {
                        RRQCols.push(i)
                        RRQRows.push(RRQ[i])
                    }
                }
                $scope.RRQTable.push({ rows: [RRQRows], cols: RRQCols });

                RRQRows = []; RRQCols = [];
                for (i in ReportData) {
                    currRow = []
                    for (j in ReportData[i]) {
                        if (j == 'Responses') {
                            for (var k in ReportData[i].Responses) {
                                if (parseInt(i) == 0)
                                    RRQCols.push("Q " + (parseInt(k) + 1));
                                currRow.push(ReportData[i].Responses[k].OptionChar);
                            }
                        }
                        else if (j != 'StudentID') {
                            if (parseInt(i)==0)
                                RRQCols.push(j)
                            currRow.push(ReportData[i][j])
                        }
                    }
                    RRQRows.push(currRow)
                }
                $scope.RRQTable.push({ rows: RRQRows, cols: RRQCols });
            
                console.log($scope.RRQTable)
                
            });
    }

    $scope.DownloadReport = function () {
        CreateReportCSV();
    }

    $scope.DownloadIntegrationReport = function () {
        CreateIntegrationReport();
    }

    function CreateReportCSV() {
        var tableDiv = document.getElementById("reportTable");
        var tables = tableDiv.getElementsByTagName('table');
        var csvString = '';
        count = 0
        for (var table of tables) {
            for (var i = 0; i < table.rows.length; i++) {
                var rowData = table.rows[i].cells;
                for (var j = 0; j < rowData.length; j++) {
                    var temp = rowData[j].textContent
                    if (temp.substring(temp.length - 3, temp.length) == '...')
                        temp = $scope.RRQTable[count]["rows"][i - 1][j];

                    csvString = csvString + temp.replace(/\s+/g, ' ').trim() + ",";
                }
                csvString = csvString.substring(0, csvString.length - 1);
                csvString = csvString + "\n";
            }
            csvString = csvString + "\n";
            count += 1;
        }
        csvString = csvString.substring(0, csvString.length - 1);

        var a = $('<a/>', {
            style: 'display:none',
            href: 'data:application/octet-stream;base64,' + btoa(csvString),
            download: 'RRQReport.csv'
        }).appendTo('body');
        a[0].click();
        a.remove();
    }

    function CreateIntegrationReport() {
        var tableDiv = document.getElementById("reportTable");
        var tables = tableDiv.getElementsByTagName('table');
        var csvString = '';
        count = 0
        cols = ['FirstName', 'LastName', 'Email']
        for (var table of $scope.RRQTable) {
            if (count == 0) {
                // First table with RRQ Info
                console.log(table.rows)
                secondRow = table.rows[0];
                var RRQNo = secondRow[1];
                cols.push(RRQNo);
                for (var temp of cols)
                    csvString = csvString + temp.replace(/\s+/g, ' ').trim() + ",";

                csvString = csvString.substring(0, csvString.length - 1);
                csvString = csvString + "\n";
            }
            else {
                // Data Table
                for (var i = 0; i < table.rows.length; i++) {
                    var rowData = table.rows[i]
                    console.log(rowData)
                    rowarr = rowData[0].split(' ')
                    rowarr.push(rowData[1])
                    rowarr.push(rowData[rowData.length - 1].toString())
                    for (temp of rowarr)
                        csvString = csvString + temp.replace(/\s+/g, ' ').trim() + ",";
                    csvString = csvString.substring(0, csvString.length - 1);
                    csvString = csvString + "\n";
                }
            }
            count += 1;
        }
        csvString = csvString.substring(0, csvString.length - 1);

        var a = $('<a/>', {
            style: 'display:none',
            href: 'data:application/octet-stream;base64,' + btoa(csvString),
            download: 'IntegrationReport.csv'
        }).appendTo('body');
        a[0].click();
        a.remove();

       
    }

    function parseURLParams(url) {
        var queryStart = url.indexOf("?") + 1,
            queryEnd = url.indexOf("#") + 1 || url.length + 1,
            query = url.slice(queryStart, queryEnd - 1),
            pairs = query.replace(/\+/g, " ").split("&"),
            parms = {}, i, n, v, nv;

        if (query === url || query === "") return;

        for (i = 0; i < pairs.length; i++) {
            nv = pairs[i].split("=", 2);
            n = decodeURIComponent(nv[0]);
            v = decodeURIComponent(nv[1]);

            if (!parms.hasOwnProperty(n)) parms[n] = [];
            parms[n].push(nv.length === 2 ? v : null);
        }
        return parms;
    }
});
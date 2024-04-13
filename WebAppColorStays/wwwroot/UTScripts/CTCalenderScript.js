function ShowCalender(Obj) {
    // Defining the intial value to the variable
    //Can take 'Date','Year','Month'
    if (Obj.CalenderType == null || Obj.CalenderType == "") { Obj.CalenderType = 'Date'; }
    if (Obj.StartYear == null || Obj.StartYear == "") { Obj.StartYear = new Date().getFullYear(); }
    if (Obj.StartMonth == null || Obj.StartMonth == "") { Obj.StartMonth = new Date().getMonth(); }
    if (Obj.Language == null || Obj.Language == "") { Obj.Language = 'en-US'; }
    if (Obj.DateFormat == null || Obj.DateFormat == "") { Obj.DateFormat = 'D-MM-YYYY'; }
    if (Obj.MonthDisplayNumber == null || Obj.MonthDisplayNumber == "") { Obj.MonthDisplayNumber = 1; }
    if (Obj.CallBackFunc == null || Obj.CallBackFunc == "") { Obj.CallBackFunc = ""; }
    if (Obj.DisablePreviousDays == null || Obj.DisablePreviousDays == "") { Obj.DisablePreviousDays = "N"; }
    if (Obj.Event == null || Obj.Event == "") { Obj.Event = null; }
    if (Obj.RangeInput == null || Obj.RangeInput == "") { Obj.RangeInput = null; }
    if (Obj.RangeSelectClass == null || Obj.RangeSelectClass == "") { Obj.RangeSelectClass = 'CalenderSelectColor'; }
    if (Obj.RangeSelectStartEndMarkCs == null || Obj.RangeSelectStartEndMarkCs == "") { Obj.RangeSelectStartEndMarkCs = 'CalenderStartEnd'; }
    if (Obj.ShowMonths == null || Obj.ShowMonths == "") { Obj.ShowMonths = 1; }
    if (Obj.RangeTheme == null || Obj.RangeTheme == "") { Obj.RangeTheme = null; }
    if (Obj.PositionCalender == null || Obj.PositionCalender == "") { Obj.PositionCalender = null; }
    if (Obj.PositionCalender != null && Obj.PositionCalender != "") {
        if (Obj.PositionCalender.StartDateId == null || Obj.PositionCalender.StartDateId == "") { Obj.PositionCalender.StartDateId = null; }
        if (Obj.PositionCalender.EndDateId == null || Obj.PositionCalender.EndDateId == "") { Obj.PositionCalender.EndDateId = null; }
    }


    Obj.SelectedStartDate = null; Obj.SelectedEndDate = null;

    var CalenderType = Obj.CalenderType;
    var StartDateInput = Obj.StartDateInput;
    var RangeInput = Obj.RangeInput;
    var EndDateInput = null;
    if (RangeInput != null) {
        if (RangeInput.StartDateInput != null) {
            StartDateInput = RangeInput.StartDateInput;
            Obj.StartDateInput = RangeInput.StartDateInput;
        }
        if (RangeInput.EndDateInput != null) {
            EndDateInput = RangeInput.EndDateInput;
            Obj.EndDateInput = RangeInput.EndDateInput;

        }
    }

    if (StartDateInput != null) {
        // Defining the intial value to the Ends
        switch (CalenderType) {
            case 'Date': document.getElementById(StartDateInput).addEventListener('click', function (e) { Obj.Event = e; DateCalender(Obj); });
                if (EndDateInput != null) { document.getElementById(EndDateInput).addEventListener('click', function (e) { Obj.Event = e; DateCalender(Obj); }); }
                break;
            case 'Year': document.getElementById(StartDateInput).addEventListener('click', function (e) { Obj.Event = e; YearCalender(Obj); });
                if (EndDateInput != null) { document.getElementById(EndDateInput).addEventListener('click', function (e) { Obj.Event = e; YearCalender(Obj); }); }
                break;
            case 'Month': document.getElementById(StartDateInput).addEventListener('click', function (e) { Obj.Event = e; MonthCalender(Obj); });
                if (EndDateInput != null) { document.getElementById(EndDateInput).addEventListener('click', function (e) { Obj.Event = e; MonthCalender(Obj); }); }
                break;
        }
    }
}

// Swipe Function Starts Here

let pageWidth = window.innerWidth || document.body.clientWidth;
let treshold = Math.max(1, Math.floor(0.01 * (pageWidth)));
let touchstartX = 0;
let touchstartY = 0;
let touchendX = 0;
let touchendY = 0;

const limit = Math.tan(45 * 1.5 / 180 * Math.PI);

function handleGesture(e, Obj) {
    let x = touchendX - touchstartX;
    let y = touchendY - touchstartY;
    let xy = Math.abs(x / y);
    let yx = Math.abs(y / x);
    if (Math.abs(x) > treshold || Math.abs(y) > treshold) {
        if (yx <= limit) {
            if (x < 0) {
                DateChangeUT(parseInt(Obj.StartYear), parseInt(Obj.StartMonth + 1), Obj);
            } else {
                DateChangeUT(parseInt(Obj.StartYear), parseInt(Obj.StartMonth - 1), Obj);
            }
        }

    }
}
//Swipe Function Ends Here
function DateCalender(Obj) {
    // Start Calender Date is Decided In Two Function 1) DateCalender 2) DateSelect
    try {
        if (Obj.Event != null || Obj.Event != "") {
            Obj.CurrentSelectedInput = Obj.Event.target.id;
            //if null then Calender Not Formed & DateArrowButton becomes nonfunctional as it picks same Date
            //that in the text box
            if (Obj.Event.target.value != null && Obj.Event.target.value != "" && Obj.DateArrowActive != 'Y') {
                Obj.StartYear = new Date(Obj.Event.target.value).getFullYear();
                Obj.StartMonth = new Date(Obj.Event.target.value).getMonth();
            }
            Obj.DateArrowActive = 'N';
        }
    } catch { }

    //Gets the current Date variables
    var today = new Date();
    var UserYear = Obj.StartYear;
    var UserMonth = Obj.StartMonth;
    var UserLang = Obj.Language;
    var StartDateInput = Obj.StartDateInput;
    var EndDateInput = Obj.EndDateInput;
    var DisableCount = null;
    var DisableDate = null;
    var DisableBeforeDateCheck = null;
    var DiasableAfterDateCheck = null;
    var DisableCalenderDate = null;
    var CurrentSelectedInput = Obj.CurrentSelectedInput;
    var SelectedStartDate = Obj.SelectedStartDate;
    var SelectedEndDate = Obj.SelectedEndDate;
    var RangeSelectClass = Obj.RangeSelectClass;
    var RangeSelectStartEndMarkCs = Obj.RangeSelectStartEndMarkCs;
    var RangeCalender = 'N';
    var RangeTheme = Obj.RangeTheme;
    var ShowMonths = Obj.ShowMonths;
    var StartCalenderPosition = null;
    var EndCalenderPosition = null;
    if (Obj.PositionCalender != null && Obj.PositionCalender != "") {
        if (Obj.PositionCalender.StartDateId != null && Obj.PositionCalender.StartDateId != "") { StartCalenderPosition = Obj.PositionCalender.StartDateId; }
        if (Obj.PositionCalender.EndDateId != null && Obj.PositionCalender.EndDateId != "") { EndCalenderPosition = Obj.PositionCalender.EndDateId; }
    }
    if (Obj.RangeInput != null) {
        RangeCalender = 'Y';
    }
    var DisableAfterDays = DisableAfterDays;


    // Disable Dates On the Basis Of Date Passed
    if (Obj.DisableDate != null && Obj.DisableDate != '') {
        DisableDate = Obj.DisableDate;
        DisableCalenderDate = true;
        if (DisableDate.Before != null && DisableDate.Before != '') {
            DisableBeforeDateCheck = new Date(DisableDate.Before);
        }
        if (DisableDate.After != null && DisableDate.After != '') {
            DiasableAfterDateCheck = new Date(DisableDate.After);
        }
    }
    // Disable Dates On the Basis Of Count
    if (Obj.DisableCount != null && Obj.DisableCount != '') {
        DisableCount = Obj.DisableCount;
        DisableCalenderDate = true;

        if (DisableCount.Before != null && DisableCount.Before != '') {
            DisableBeforeDateCheck = new Date(today.getFullYear(), today.getMonth(), (today.getDate() - DisableCount.Before));
        }
        if (DisableCount.After != null && DisableCount.After != '') {
            DiasableAfterDateCheck = new Date(today.getFullYear(), today.getMonth(), (today.getDate() + DisableCount.After));
        }
    }


    // Checking if the Range Input Calender then if the enddate is selected then disable dates before the check In Dates
    if (Obj.RangeInput != null) {
        if (CurrentSelectedInput == Obj.EndDateInput) {
            DisableBeforeDateCheck = new Date(document.getElementById(Obj.StartDateInput).value);
        }
    }


    // Disable Dates On the Basis DisableAfterDays
    if (DisableAfterDays != null && DisableAfterDays != '') {
        if (CurrentSelectedInput == Obj.EndDateInput) {
            DisableCalenderDate = true;
            DisableBeforeDateCheck = new Date(document.getElementById(Obj.StartDateInput).value);
            DiasableAfterDateCheck = new Date(DisableBeforeDateCheck.getFullYear(), DisableBeforeDateCheck.getMonth(), parseInt(DisableBeforeDateCheck.getDate() + DisableAfterDays));


        }
    }

    // Checking if the Range Input Calender then if the enddate is selected then disable dates before the check In Dates
    if (Obj.RangeInput != null && DisableAfterDays == null && Obj.DisableCount == null && Obj.DisableDate == null) {
        if (CurrentSelectedInput == Obj.EndDateInput) {
            DisableCalenderDate = true;
            DisableBeforeDateCheck = new Date(document.getElementById(Obj.StartDateInput).value);

        }
    }
    //Ends Disable Date Check

    function GetDayName(dateStr) {
        var date = new Date(dateStr);
        return date.toLocaleDateString(UserLang, { weekday: 'long' });
    }
    var RangeContainer = null;
    var ThemeHeader = null;
    var ThemeFooter = null;
    var ThemeMonthBar = null;

    if (RangeTheme != null) {
        var getTheme = RangeTheme1(Obj);
        RangeContainer = document.createElement('div');
        RangeContainer.setAttribute('class', 'CalenderRangeContainer')
        ThemeHeader = getTheme.Head;
        ThemeFooter = getTheme.Foot;
        ThemeMonthBar = getTheme.MonthBar;
    }

    var CalenderDivContainer = document.createElement('div');
    CalenderDivContainer.setAttribute("class", " DyFx Gp5");

    // Adding Event Listner for the swipe functionality
    CalenderDivContainer.addEventListener('touchstart', function (event) {
        touchstartX = event.changedTouches[0].screenX;
        touchstartY = event.changedTouches[0].screenY;
    }, false);

    CalenderDivContainer.addEventListener('touchend', function (event) {
        touchendX = event.changedTouches[0].screenX;
        touchendY = event.changedTouches[0].screenY;
        handleGesture(event, Obj);
    }, false);
    // Ends Adding Event Listner for the swipe functionality


    //Main Head Ends

    //Create DayNameRow

    var WeekDay = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];


    for (var mx = 0; mx < ShowMonths; mx++) {
        const CalenderDiv = document.createElement("div");
        CalenderDiv.setAttribute('class', 'CalenderDiv');
        CalenderDiv.setAttribute('CalenderCT', 'UTCalenderDateDiv');
        CalenderDiv.setAttribute('id', 'UTCalenderDateDiv' + mx);

        var month = new Date(UserYear, UserMonth).toLocaleString(UserLang, { month: 'short' });
        // Adding Calender Rows
        var monthDate = 1;

        var CalenderCurrentStartDate = new Date(UserYear, parseInt(UserMonth + mx), monthDate)
        var CalenderDayName = GetDayName(CalenderCurrentStartDate);
        var CalenderYear = CalenderCurrentStartDate.getFullYear();
        var CalenderMonth = CalenderCurrentStartDate.getMonth();
        var CalenderMonthName = new Date(CalenderYear, CalenderMonth).toLocaleString(UserLang, { month: 'short' });
        var MonthDays = new Date(CalenderYear, CalenderMonth + 1, 0).getDate();

        //alert(CalenderCurrentStartDate.getMonth());
        //For Comparing Month Start Date DayName
        var WeekDayName = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];


        if (RangeTheme == null) {
            //Creating the Calender Header with Icon and Current Month & Year
            //Main Head Div
            const CalenderHeadDiv = document.createElement('div');
            CalenderHeadDiv.setAttribute('class', 'CalenderHead CalenderRowHighlight');
            CalenderDiv.appendChild(CalenderHeadDiv);
            //Main Head child Add
            const CalenderIconMonthDiv = document.createElement('div');
            CalenderIconMonthDiv.setAttribute('class', 'CalenderUpDownIcon');
            CalenderHeadDiv.appendChild(CalenderIconMonthDiv);

            //Start Month Change Button
            const SubMonthDiv = document.createElement('div');
            SubMonthDiv.addEventListener('click', function () { DateChangeUT(parseInt(UserYear), parseInt(UserMonth - 1), Obj); });
            SubMonthDiv.setAttribute('class', "Cr-uPr");
            SubMonthDiv.classList.add('FxCrAnJy');
            CalenderIconMonthDiv.appendChild(SubMonthDiv);
            SubMonthDiv.innerHTML = '<i class="UT-ArrowLeftCircleSolid" > </i>';

            const CalenderCurrentMonth = document.createElement('div');
            CalenderCurrentMonth.addEventListener('click', function () { MonthCalender(Obj) });
            CalenderCurrentMonth.setAttribute('class', 'Cr-uPr');
            CalenderCurrentMonth.classList.add('FxGw1');
            CalenderCurrentMonth.classList.add('FxCrAnJy');

            CalenderCurrentMonth.innerHTML = month;
            CalenderIconMonthDiv.appendChild(CalenderCurrentMonth);
            const AddMonthDiv = document.createElement('div');
            AddMonthDiv.addEventListener('click', function () { DateChangeUT(parseInt(UserYear), parseInt(UserMonth + 1), Obj); });
            AddMonthDiv.setAttribute('class', "Cr-uPr");
            AddMonthDiv.classList.add('FxCrAnJy');
            CalenderIconMonthDiv.appendChild(AddMonthDiv);
            AddMonthDiv.innerHTML = '<i class="UT-ArrowRightCircleSolid" >';

            // End Month Change Button


            const CalenderIconYearDiv = document.createElement('div');
            CalenderIconYearDiv.setAttribute('class', 'CalenderUpDownIcon');
            CalenderHeadDiv.appendChild(CalenderIconYearDiv);

            ///Start Year Change Button Function
            const SubYearDiv = document.createElement('div');
            SubYearDiv.addEventListener('click', function () { DateChangeUT(parseInt(UserYear - 1), parseInt(UserMonth), Obj); });
            SubYearDiv.setAttribute('class', "Cr-uPr");
            SubYearDiv.classList.add('FxCrAnJy');
            CalenderIconYearDiv.appendChild(SubYearDiv);
            SubYearDiv.innerHTML = '<i class="UT-ArrowLeftCircleSolid" > </i>';

            const CalenderCurrentYear = document.createElement('div');
            CalenderCurrentYear.addEventListener('click', function () { YearCalender(Obj) });
            CalenderCurrentYear.setAttribute('class', 'Cr-uPr');
            CalenderCurrentYear.classList.add('FxGw1');
            CalenderCurrentYear.classList.add('FxCrAnJy');

            //Checks Leap Year
            if (new Date(UserYear, 1, 29).getDate() === 29) {
                CalenderCurrentYear.innerHTML = UserYear;
            }
            else {
                CalenderCurrentYear.innerHTML = UserYear;
            }
            CalenderIconYearDiv.appendChild(CalenderCurrentYear);


            const AddYearDiv = document.createElement('div');
            AddYearDiv.addEventListener('click', function () { DateChangeUT(parseInt(UserYear + 1), parseInt(UserMonth), Obj); });
            AddYearDiv.setAttribute('class', "Cr-uPr");
            AddYearDiv.classList.add('FxCrAnJy');
            CalenderIconYearDiv.appendChild(AddYearDiv);
            AddYearDiv.innerHTML = '<i class="UT-ArrowRightCircleSolid" > </i>';
            ///End Year Change Button Function

        }
        else {

            var CalenderMonthDiv = document.createElement('div');
            CalenderMonthDiv.setAttribute('class', 'Cr-uPr  FxCrAnJy PgTpBm10 Wh100p FtWt700');
            var CalenderMonthIcon = document.createElement('i');
            CalenderMonthIcon.setAttribute('class', 'UT-CalenderSmallLine');
            CalenderMonthIcon.innerHTML = " ";
            CalenderMonthDiv.appendChild(CalenderMonthIcon);
            CalenderMonthDiv.appendChild(document.createTextNode(" " + CalenderMonthName + " - " + CalenderYear));
            CalenderDiv.appendChild(CalenderMonthDiv);

        }

        const CalenderWeekDayDiv = document.createElement('div');
        CalenderWeekDayDiv.setAttribute('class', 'CalenderDayName CalenderRowHighlight');
        CalenderDiv.appendChild(CalenderWeekDayDiv);

        for (var i = 0; i < WeekDay.length; i++) {
            const DayName = document.createElement('div');
            DayName.innerHTML = WeekDay[i];
            CalenderWeekDayDiv.appendChild(DayName);
        }

        for (var i = 0; i < 6; i++) {
            var CalenderRowDiv = document.createElement('div');
            CalenderRowDiv.setAttribute('class', 'CalenderDayRow');

            for (var weekDay = 0; weekDay < 7; weekDay++) {
                var CalenderRowDayDiv = document.createElement('div');

                let DateVar = monthDate;
                let MonthVar = CalenderMonth;
                let YearVar = CalenderYear;
                if (CalenderDayName == WeekDayName[weekDay] && monthDate <= MonthDays) {
                    CalenderRowDayDiv.setAttribute('class', 'CalenderDay');
                    CalenderRowDayDiv.classList.add('CelebrateDay');
                    CalenderRowDayDiv.setAttribute('CelebrateDate', CalenderYear + '-' + parseInt(CalenderMonth + 1) + '-' + monthDate);
                    CalenderRowDayDiv.innerHTML = monthDate;
                    var CreatedDate = new Date(CalenderYear, CalenderMonth, monthDate);
                    // Check For the PreviousDisable And StartEndDisable And DisableAfter

                    if (DisableCalenderDate == true) {
                        if (DisableBeforeDateCheck != null && DiasableAfterDateCheck != null) {
                            if (CreatedDate > DisableBeforeDateCheck && CreatedDate < DiasableAfterDateCheck) {
                                CalenderRowDayDiv.addEventListener('click', function () { DateSelect(YearVar, MonthVar, DateVar, Obj) });
                                if (RangeCalender == 'Y') {
                                    CalenderRowDayDiv.addEventListener('mouseover', function (event) { HighlightDate(Obj, event); });
                                }
                            }
                            else {
                                CalenderRowDayDiv.setAttribute('class', 'CalenderDayDisable');
                            }
                        }
                        else if (DisableBeforeDateCheck != null && DiasableAfterDateCheck == null) {
                            if (CreatedDate > DisableBeforeDateCheck) {
                                CalenderRowDayDiv.addEventListener('click', function () { DateSelect(YearVar, MonthVar, DateVar, Obj) });
                                if (RangeCalender == 'Y') {
                                    CalenderRowDayDiv.addEventListener('mouseover', function (event) { HighlightDate(Obj, event); });
                                }
                            }
                            else {
                                CalenderRowDayDiv.setAttribute('class', 'CalenderDayDisable');
                            }
                        }
                        else if (DisableBeforeDateCheck == null && DiasableAfterDateCheck != null) {
                            if (CreatedDate < DiasableAfterDateCheck) {
                                CalenderRowDayDiv.addEventListener('click', function () { DateSelect(YearVar, MonthVar, DateVar, Obj) });
                                if (RangeCalender == 'Y') {
                                    CalenderRowDayDiv.addEventListener('mouseover', function (event) { HighlightDate(Obj, event); });
                                }
                            }
                            else {
                                CalenderRowDayDiv.setAttribute('class', 'CalenderDayDisable');
                            }
                        }
                    }
                    else {
                        CalenderRowDayDiv.addEventListener('click', function () { DateSelect(YearVar, MonthVar, DateVar, Obj) });

                    }

                    //Active Day Highlight + ", '" + StartDateInput + "' )");
                    if ((new Date(today.getFullYear(), today.getMonth(), today.getDate())).getTime() === (new Date(YearVar, MonthVar, DateVar)).getTime()) {
                        CalenderRowDayDiv.classList.add("CalenderDayActiveUT");
                    }
                    //Select Range
                    if (CreatedDate >= SelectedStartDate && CreatedDate <= SelectedEndDate) {
                        CalenderRowDayDiv.classList.add(RangeSelectClass);
                        if (CreatedDate.toDateString() === SelectedStartDate.toDateString() || CreatedDate.toDateString() === SelectedEndDate.toDateString()) {
                            CalenderRowDayDiv.classList.add(RangeSelectStartEndMarkCs);
                        }
                    }

                    monthDate++;
                    CalenderDayName = GetDayName(new Date(CalenderYear, CalenderMonth, monthDate));
                }
                else {
                    CalenderRowDayDiv.innerHTML = " ";
                }
                CalenderRowDiv.appendChild(CalenderRowDayDiv);


            }
            CalenderDiv.appendChild(CalenderRowDiv);

            // Month Date exceed the total number of days in the month break loop
            if (monthDate > MonthDays) {
                break;
            }
        }
        //Adding Left Arrow
        if (RangeTheme != null) {

            if (mx == 0) {
                var ArrowDiv = document.createElement('div');
                ArrowDiv.setAttribute('class', 'FxCrAnJy Mn5');
                var ArrowSpan = document.createElement('span');
                ArrowSpan.setAttribute('class', 'FtSz24 CrTe FxCrAnJy FtWt800 Cr-uPr');
                ArrowSpan.addEventListener('click', function () { DateChangeUT(parseInt(UserYear), parseInt(UserMonth - 1), Obj); });
                var IconLeft = document.createElement('i');
                IconLeft.setAttribute('class', 'UT-ArrowLeftCircleSolid');

                ArrowSpan.appendChild(IconLeft);
                ArrowDiv.appendChild(ArrowSpan);
                CalenderDivContainer.appendChild(ArrowDiv);

            }
        }
        //Adding Calenders
        CalenderDivContainer.appendChild(CalenderDiv);
        //Adding RIght Arrow
        if (RangeTheme != null) {

            if (mx == parseInt(ShowMonths - 1)) {
                var ArrowDiv = document.createElement('div');
                ArrowDiv.setAttribute('class', 'FxCrAnJy Mn5');
                var ArrowSpan = document.createElement('span');
                ArrowSpan.setAttribute('class', 'FtSz24 CrTe FxCrAnJy FtWt800 Cr-uPr');
                ArrowSpan.addEventListener('click', function () { DateChangeUT(parseInt(UserYear), parseInt(UserMonth + 1), Obj); });

                var IconRight = document.createElement('i');
                IconRight.setAttribute('class', 'UT-ArrowRightCircleSolid');

                ArrowSpan.appendChild(IconRight);
                ArrowDiv.appendChild(ArrowSpan);
                CalenderDivContainer.appendChild(ArrowDiv);
            }
        }
    }


    if (RangeTheme != null) {
        RangeContainer.appendChild(ThemeHeader);

        RangeContainer.appendChild(CalenderDivContainer);
        RangeContainer.appendChild(ThemeFooter);

        Obj.CreatedCalender = RangeContainer;

        // Checking Where To Open The Calender RangeCalPosition

        if (CurrentSelectedInput == StartDateInput && StartCalenderPosition != null) {
            Obj.PostionCalender = StartCalenderPosition;
            AddCalender(Obj);

        }
        else if (CurrentSelectedInput == EndDateInput && EndCalenderPosition != null) {
            Obj.PostionCalender = EndCalenderPosition;
            AddCalender(Obj);
        }
        else if (StartCalenderPosition == null && EndCalenderPosition == null) {
            Obj.PostionCalender = CurrentSelectedInput;
            AddCalender(Obj);

        }
    }
    else {
        Obj.CreatedCalender = CalenderDivContainer;
        Obj.PostionCalender = CurrentSelectedInput;
        AddCalender(Obj);
    }
}

function RangeTheme1(Obj) {
    var SelectedRange = null;
    var StartDateFo = null;
    var EndDateFo = null;
    var StartMonthName = null;
    var EndMonthName = null;
    var StartYear = null;
    var EndYear = null;

    var StartDate = Obj.SelectedStartDate;
    var EndDate = Obj.SelectedEndDate;
    var CurrentSelectedInput = Obj.CurrentSelectedInput;
    var StartDateInput = Obj.StartDateInput;
    var RangeInput = Obj.RangeInput;
    var EndDateInput = Obj.EndDateInput;



    if (StartDate != null) {
        StartDateFo = StartDate.getDate() + "/" + parseInt(StartDate.getMonth() + 1) + "/" + StartDate.getFullYear();
        StartMonthName = StartDate.toLocaleString(Obj.UserLang, { month: 'short' });
        StartYear = StartDate.getFullYear();
    }
    if (EndDate != null) {
        EndDateFo = EndDate.getDate() + "/" + parseInt(EndDate.getMonth() + 1) + "/" + EndDate.getFullYear();
        EndMonthName = EndDate.toLocaleString(Obj.UserLang, { month: 'short' });
        EndYear = EndDate.getFullYear();
    }

    if (StartDate != null && EndDate != null) {
        SelectedRange = (EndDate - StartDate) / (1000 * 60 * 60 * 24);
    }


    var CalenderRangeHead = document.createElement("div");
    CalenderRangeHead.setAttribute('class', 'CalenderRangeHead');

    var CalenderRangeCalDiv = document.createElement("div");

    var RangeSelected = document.createElement('h2');
    RangeSelected.setAttribute('id', 'RangeDuration');

    if (SelectedRange != null) {
        if (SelectedRange == 1) {
            RangeSelected.innerHTML = SelectedRange + ' Night' + " / " + parseInt(SelectedRange + 1) + " Days";
        } else { RangeSelected.innerHTML = SelectedRange + ' Nights' + " / " + parseInt(SelectedRange + 1) + " Days"; }

    } else {
        RangeSelected.innerHTML = 'No Selection';

    }
    CalenderRangeCalDiv.appendChild(RangeSelected);


    var RangeFromTo = document.createElement('p');
    RangeFromTo.setAttribute('class', 'CalenderRangeHeadDate');
    RangeFromTo.setAttribute('id', 'RangeFromTo');
    if (SelectedRange != null) {
        RangeFromTo.innerHTML = StartDateFo + " To " + EndDateFo;

    } else {
        RangeFromTo.innerHTML = 'No Date';

    }
    CalenderRangeCalDiv.appendChild(RangeFromTo);

    //First Addition to Calender Range Head
    CalenderRangeHead.appendChild(CalenderRangeCalDiv);

    // Calender Start Date Input Starts
    var CalenderRangeHeadDateDiv = document.createElement('div');
    CalenderRangeHeadDateDiv.setAttribute('class', 'CalenderRangeHeadDateDiv');

    var CalenderRangeHeadDateDivDate = document.createElement('div');
    CalenderRangeHeadDateDivDate.setAttribute('class', 'CalenderRangeHeadDateDivDate');
    if (RangeInput != null) {
        if (CurrentSelectedInput == StartDateInput) {
            CalenderRangeHeadDateDivDate.classList.add('FsActive');
        }
    }

    var CalenderRangeHeadDateDivDateCap = document.createElement('div');
    CalenderRangeHeadDateDivDateCap.setAttribute('class', 'CalenderRangeHeadDateDivDateCap');
    CalenderRangeHeadDateDivDateCap.innerHTML = 'Check In';

    CalenderRangeHeadDateDivDate.appendChild(CalenderRangeHeadDateDivDateCap);

    var NormalDiv = document.createElement('div');
    NormalDiv.setAttribute('class', 'PnRe');
    var CalenderRangeHeadDateDivDateInp = document.createElement('input');
    CalenderRangeHeadDateDivDateInp.setAttribute('class', 'CalenderRangeHeadDateDivDateInp');
    CalenderRangeHeadDateDivDateInp.setAttribute('id', 'CalenderStartDateInput');
    CalenderRangeHeadDateDivDateInp.addEventListener('focus', function () { SelectStartOrEnd(Obj, 'Start'); });
    CalenderRangeHeadDateDivDateInp.setAttribute('placeholder', "DD/MM/YY");
    if (StartDateFo != null) {
        CalenderRangeHeadDateDivDateInp.value = StartDateFo;
    }
    NormalDiv.appendChild(CalenderRangeHeadDateDivDateInp);

    var CalenderRangeHeadDateDivDateInpClear = document.createElement('div');
    CalenderRangeHeadDateDivDateInpClear.setAttribute('class', 'CalenderRangeHeadDateDivDateInpClear');

    NormalDiv.appendChild(CalenderRangeHeadDateDivDateInpClear);

    CalenderRangeHeadDateDivDate.appendChild(NormalDiv);
    CalenderRangeHeadDateDiv.appendChild(CalenderRangeHeadDateDivDate);

    // Calender Start Date Input Ends

    //Calender End Date Input Starts
    var CalenderRangeHeadDateDivDate2 = document.createElement('div');
    CalenderRangeHeadDateDivDate2.setAttribute('class', 'CalenderRangeHeadDateDivDate');
    if (RangeInput != null) {
        if (CurrentSelectedInput == EndDateInput) {
            CalenderRangeHeadDateDivDate2.classList.add('FsActive');
        }
    }

    var CalenderRangeHeadDateDivDateCap2 = document.createElement('div');
    CalenderRangeHeadDateDivDateCap2.setAttribute('class', 'CalenderRangeHeadDateDivDateCap');
    CalenderRangeHeadDateDivDateCap2.innerHTML = 'Check Out';

    CalenderRangeHeadDateDivDate2.appendChild(CalenderRangeHeadDateDivDateCap2);

    var NormalDiv2 = document.createElement('div');
    NormalDiv2.setAttribute('class', 'PnRe');
    var CalenderRangeHeadDateDivDateInp2 = document.createElement('input');
    CalenderRangeHeadDateDivDateInp2.setAttribute('class', 'CalenderRangeHeadDateDivDateInp');
    CalenderRangeHeadDateDivDateInp2.setAttribute('id', 'CalenderStartDateInput');
    CalenderRangeHeadDateDivDateInp2.addEventListener('focus', function () { SelectStartOrEnd(Obj, 'End'); });
    CalenderRangeHeadDateDivDateInp2.setAttribute('oninput', 'CalenderStartCloseButton()');
    CalenderRangeHeadDateDivDateInp2.setAttribute('placeholder', "DD/MM/YY");
    if (EndDateFo != null) {
        CalenderRangeHeadDateDivDateInp2.value = EndDateFo;
    }
    NormalDiv2.appendChild(CalenderRangeHeadDateDivDateInp2);

    var CalenderRangeHeadDateDivDateInpClear2 = document.createElement('div');
    CalenderRangeHeadDateDivDateInpClear2.setAttribute('class', 'CalenderRangeHeadDateDivDateInpClear');

    NormalDiv2.appendChild(CalenderRangeHeadDateDivDateInpClear2);

    CalenderRangeHeadDateDivDate2.appendChild(NormalDiv2);
    CalenderRangeHeadDateDiv.appendChild(CalenderRangeHeadDateDivDate2);

    // Calender End Date Input Ends

    //Add To Parent Div
    CalenderRangeHead.appendChild(CalenderRangeHeadDateDiv);
    //Ends

    //// Calender Range Foot Starts From Here
    var CalenderRangeFoot = document.createElement('div');
    CalenderRangeFoot.setAttribute('class', 'CalenderRangeFoot');
    var Div1 = document.createElement('div');
    CalenderRangeFoot.appendChild(Div1);
    var CalenderRangeFootBnDv = document.createElement('div');
    CalenderRangeFootBnDv.setAttribute('class', 'CalenderRangeFootBnDv');

    var ClearAnchor = document.createElement('a');
    ClearAnchor.setAttribute('class', 'CalenderRangeFootClearDate');
    ClearAnchor.innerText = "Clear Dates";
    ClearAnchor.addEventListener('click', function () { ClearCalenderDates(Obj); });
    CalenderRangeFootBnDv.appendChild(ClearAnchor);

    var CalenderRangeFootCsBn = document.createElement('div');
    CalenderRangeFootCsBn.setAttribute('class', 'CalenderRangeFootCsBn');
    CalenderRangeFootCsBn.innerHTML = 'Close';
    CalenderRangeFootCsBn.addEventListener('click', function () { CloseCalender(Obj); });
    CalenderRangeFootBnDv.append(CalenderRangeFootCsBn);
    CalenderRangeFoot.appendChild(CalenderRangeFootBnDv);
    //// Calender Range Foot Ends  Here

    //// Calender Month Head Starts From Here

    var CalenderMonthHeadCont = document.createElement('div');
    CalenderMonthHeadCont.classList.add('DyFx');
    CalenderMonthHeadCont.classList.add('FxJySeBn');

    var CalenderMonthHead1 = document.createElement('div');
    CalenderMonthHead1.classList.add('CalenderMonthHead');
    var CalenderMonthHead1Child = document.createElement('div');
    CalenderMonthHead1Child.setAttribute('class', 'Wh100p');
    var CalenderMonthHead1ChildSpan = document.createElement('span');
    //Set on Click Event on Arrow
    CalenderMonthHead1ChildSpan.innerHTML = '<i class="UT-ArrowLeft"></i>';
    CalenderMonthHead1Child.appendChild(CalenderMonthHead1ChildSpan);

    // CalenderMonthHead1ChildsDiv starts
    var CalenderMonthHead1ChildsDiv = document.createElement('div');
    CalenderMonthHead1ChildsDiv.setAttribute('class', 'Cr-uPr FxGw1 FxCrAnJy');
    var CalenderMonthHead1ChildsDivIcon = document.createElement('i');
    CalenderMonthHead1ChildsDivIcon.setAttribute('class', 'UT-CalenderSmallLine');
    CalenderMonthHead1ChildsDivIcon.classList.add('FtWt700');
    CalenderMonthHead1ChildsDivIcon.innerHTML = StartMonthName + " " + StartYear;
    CalenderMonthHead1ChildsDiv.appendChild(CalenderMonthHead1ChildsDivIcon);
    // CalenderMonthHead1ChildsDiv Ends

    CalenderMonthHead1Child.appendChild(CalenderMonthHead1ChildsDiv);
    CalenderMonthHead1.appendChild(CalenderMonthHead1Child);
    CalenderMonthHeadCont.appendChild(CalenderMonthHead1);

    var CalenderMonthHead2 = document.createElement('div');
    CalenderMonthHead2.classList.add('CalenderMonthHead');
    var CalenderMonthHead2Child = document.createElement('div');
    CalenderMonthHead2Child.setAttribute('class', 'Wh100p');


    // CalenderMonthHead2ChildsDiv starts
    var CalenderMonthHead2ChildsDiv = document.createElement('div');
    CalenderMonthHead2ChildsDiv.setAttribute('class', 'Cr-uPr FxGw1 FxCrAnJy');
    var CalenderMonthHead2ChildsDivIcon = document.createElement('i');
    CalenderMonthHead2ChildsDivIcon.setAttribute('class', 'UT-CalenderSmallLine');
    CalenderMonthHead2ChildsDivIcon.classList.add('FtWt700');
    CalenderMonthHead2ChildsDivIcon.innerHTML = EndMonthName + " " + EndYear;
    CalenderMonthHead2ChildsDiv.appendChild(CalenderMonthHead2ChildsDivIcon);
    // CalenderMonthHead2ChildsDiv Ends
    CalenderMonthHead2Child.appendChild(CalenderMonthHead2ChildsDiv);

    var CalenderMonthHead2ChildSpan = document.createElement('span');
    //Set on Click Event on Arrow
    CalenderMonthHead2ChildSpan.innerHTML = '<i class="UT-ArrowRight"></i>';
    CalenderMonthHead2Child.appendChild(CalenderMonthHead2ChildSpan);

    CalenderMonthHead2.appendChild(CalenderMonthHead2Child);
    CalenderMonthHeadCont.appendChild(CalenderMonthHead2);


    var theme = {
        Head: CalenderRangeHead,
        Foot: CalenderRangeFoot,
        MonthBar: CalenderMonthHeadCont
    };

    return theme;

}

function HighlightDate(Obj, event) {
    var SelectedStartDate = Obj.SelectedStartDate;
    var highlightDiv = document.getElementsByClassName('CalenderDay');
    var highLightClass = Obj.RangeSelectClass;
    var HoverDate = new Date(event.target.getAttribute('CelebrateDate'));
    if (Obj.CurrentSelectedInput != Obj.StartDateInput) {

        if (SelectedStartDate != "" && SelectedStartDate != null) {
            for (var i = 0; i < highlightDiv.length; i++) {
                var date = new Date(highlightDiv[i].getAttribute('CelebrateDate'));
                if (date >= SelectedStartDate && date <= HoverDate) {

                    if (!highlightDiv[i].classList.contains(highLightClass)) {
                        highlightDiv[i].classList.add(highLightClass);
                    }
                }
                else {
                    if (highlightDiv[i].classList.contains(highLightClass)) {
                        highlightDiv[i].classList.remove(highLightClass);
                    }

                }
            }
        }
    }
}

function MonthCalender(Obj) {
    YearInt = Obj.StartYear;
    try {
        if (Obj.Event != null || Obj.Event != "") {
            Obj.CurrentSelectedInput = Obj.Event.target.id;
        }
    } catch { }
    var CurrentSelectedInput = Obj.CurrentSelectedInput;
    const CalenderDiv = document.createElement("div");
    CalenderDiv.setAttribute('class', 'CalenderMonthDiv');
    CalenderDiv.setAttribute('id', 'UTCalenderMonthDiv');


    //Creating the Calender Header with Icon and Current Month & YearInt
    //Main Head Div
    const CalenderHeadDiv = document.createElement('div');
    CalenderHeadDiv.setAttribute('class', 'CalenderMonthHead CalenderRowHighlight');
    CalenderDiv.appendChild(CalenderHeadDiv);
    //Main Head child Add
    const CalenderIconMonthDiv = document.createElement('div');
    CalenderHeadDiv.appendChild(CalenderIconMonthDiv);
    CalenderIconMonthDiv.innerHTML = '<i class="UT-MonthNumberLine FtSz18"></i>' + ' &nbsp; ' + ' Months'

    const CalenderCurrentYear = document.createElement('div');
    CalenderCurrentYear.addEventListener('click', function () { YearCalender(Obj); });
    CalenderCurrentYear.setAttribute('class', 'Cr-uPr');
    //Checks Leap Year
    if (new Date(YearInt, 1, 29).getDate() === 29) {
        CalenderCurrentYear.innerHTML = YearInt + '&nbsp' + 'L' + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
    }
    else {
        CalenderCurrentYear.innerHTML = YearInt + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
    }
    CalenderHeadDiv.appendChild(CalenderCurrentYear);


    //Add Months
    var MonthName = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

    var monthindex = 0;

    for (var r = 0; r < 3; r++) {

        const CalenderMonthDiv = document.createElement('div');
        CalenderMonthDiv.setAttribute('class', 'CalenderMonthRow');
        CalenderDiv.appendChild(CalenderMonthDiv);

        for (var i = 0; i < 4; i++) {
            const MonthInitials = document.createElement('div');
            if (MonthName[monthindex] == new Date().toLocaleString('en-US', { month: 'short' })) {
                MonthInitials.setAttribute('class', 'CalenderDayActiveUT');
            }

            MonthInitials.innerHTML = MonthName[monthindex];
            var passMonth = monthindex;
            const newObj = JSON.parse(JSON.stringify(Obj));
            newObj.StartMonth = passMonth;
            MonthInitials.addEventListener('click', function () { DateCalender(newObj) });

            CalenderMonthDiv.appendChild(MonthInitials);
            monthindex++;
        }
    }
    Obj.CreatedCalender = CalenderDiv;
    Obj.PostionCalender = CurrentSelectedInput;
    AddCalender(Obj);
}

function YearCalender(Obj) {
    var YearInt = Obj.StartYear;
    const CalenderDiv = document.createElement("div");

    try {
        if (Obj.Event != null || Obj.Event != "") {
            Obj.CurrentSelectedInput = Obj.Event.target.id;
        }
    } catch { }
    if (Obj.Event == null || Obj.Event == "") { Obj.Event = null; }
    var CurrentSelectedInput = Obj.CurrentSelectedInput;

    CalenderDiv.setAttribute('class', 'CalenderDiv');
    CalenderDiv.setAttribute('id', 'UTCalenderYearDiv');


    //Creating the Calender Header with Icon and Current Month & YearInt
    //Main Head Div
    var CalenderHeadDiv = document.createElement('div');
    CalenderHeadDiv.setAttribute('class', 'CalenderHead CalenderRowHighlight');
    CalenderDiv.appendChild(CalenderHeadDiv);
    //Main Head child Add
    var CalenderIconDownYearDiv = document.createElement('div');
    CalenderIconDownYearDiv.setAttribute('class', 'CalenderUpDownIcon');
    CalenderHeadDiv.appendChild(CalenderIconDownYearDiv);

    var YearJumpDn3 = document.createElement('div');
    const ObjJumpDn3 = JSON.parse(JSON.stringify(Obj));
    ObjJumpDn3.StartYear = parseInt(YearInt - 25);
    YearJumpDn3.addEventListener('click', function () { YearCalender(ObjJumpDn3) });
    YearJumpDn3.setAttribute('class', "Cr-uPr");
    CalenderIconDownYearDiv.appendChild(YearJumpDn3);
    YearJumpDn3.innerHTML = '<i class="UT-ArrowDownOneLineCircleSolid" >';

    var YearJumpDn2 = document.createElement('div');
    const ObjJumpDn2 = JSON.parse(JSON.stringify(Obj));
    ObjJumpDn2.StartYear = parseInt(YearInt - 50);
    YearJumpDn2.addEventListener('click', function () { YearCalender(ObjJumpDn2) });
    YearJumpDn2.setAttribute('class', "Cr-uPr");
    CalenderIconDownYearDiv.appendChild(YearJumpDn2);
    YearJumpDn2.innerHTML = '<i class="UT-ArrowDownTwoLineCircleSolid" > </i>';

    var YearJumpDn1 = document.createElement('div');
    const ObjJumpDn1 = JSON.parse(JSON.stringify(Obj));
    ObjJumpDn1.StartYear = parseInt(YearInt - 75);
    YearJumpDn1.addEventListener('click', function () { YearCalender(ObjJumpDn1) });
    YearJumpDn1.setAttribute('class', "Cr-uPr");
    CalenderIconDownYearDiv.appendChild(YearJumpDn1);
    YearJumpDn1.innerHTML = '<i class="UT-ArrowDownThreeLineCircleSolid" > </i>';


    const CalenderYears = document.createElement('div');
    CalenderYears.innerHTML = 'Years';
    CalenderHeadDiv.appendChild(CalenderYears);

    var CalenderIconYearDiv = document.createElement('div');
    CalenderIconYearDiv.setAttribute('class', 'CalenderUpDownIcon');
    CalenderHeadDiv.appendChild(CalenderIconYearDiv);

    var YearJump3 = document.createElement('div');

    const ObjJump3 = JSON.parse(JSON.stringify(Obj));
    ObjJump3.StartYear = parseInt(YearInt + 75);
    YearJump3.addEventListener('click', function () { YearCalender(ObjJump3) });
    YearJump3.setAttribute('class', "Cr-uPr");
    CalenderIconYearDiv.appendChild(YearJump3);
    YearJump3.innerHTML = '<i class="UT-ArrowUpThreeLineCircleSolid" >';

    var YearJump2 = document.createElement('div');
    const ObjJump2 = JSON.parse(JSON.stringify(Obj));
    ObjJump2.StartYear = parseInt(YearInt + 50);
    YearJump2.addEventListener('click', function () { YearCalender(ObjJump2) });
    YearJump2.setAttribute('class', "Cr-uPr");
    CalenderIconYearDiv.appendChild(YearJump2);
    YearJump2.innerHTML = '<i class="UT-ArrowUpTwoLineCircleSolid" > </i>';

    var YearJump1 = document.createElement('div');
    const ObjJump1 = JSON.parse(JSON.stringify(Obj));
    ObjJump1.StartYear = parseInt(YearInt + 25);
    YearJump1.addEventListener('click', function () { YearCalender(ObjJump1) });
    YearJump1.setAttribute('class', "Cr-uPr");
    CalenderIconYearDiv.appendChild(YearJump1);
    YearJump1.innerHTML = '<i class="UT-ArrowUpOneLineCircleSolid" > </i>';


    //Main Head Ends
    var yearindex = 0;
    for (var r = 0; r < 5; r++) {
        const CalenderYearRowDiv = document.createElement('div');
        CalenderYearRowDiv.setAttribute('class', 'CalenderYearRow');
        CalenderDiv.appendChild(CalenderYearRowDiv);

        for (var i = 0; i < 5; i++) {
            var yearUd = YearInt + yearindex;
            const YearInit = document.createElement('div');
            YearInit.innerHTML = yearUd;
            if (yearUd == new Date().getFullYear()) { YearInit.setAttribute('class', 'CalenderDayActiveUT') }

            var passYear = parseInt(YearInt + yearindex);
            const newObj = JSON.parse(JSON.stringify(Obj));
            newObj.StartYear = passYear;
            YearInit.addEventListener('click', function () { MonthCalender(newObj) });

            CalenderYearRowDiv.appendChild(YearInit);
            yearindex++;
        }
    }
    Obj.CreatedCalender = CalenderDiv;
    Obj.PostionCalender = CurrentSelectedInput;
    AddCalender(Obj);

}

//Adds Calender To UI
function AddCalender(Obj) {
    var Calender = Obj.CreatedCalender;
    var SingleCalender = Obj.StartDateInput;
    var RangeCalender = Obj.RangeInput;
    var CurrentInput = Obj.CurrentSelectedInput;
    var OpenCalenderPosition = Obj.PostionCalender;

    // Removing the Previous Created Calender
    CloseCalender(Obj);
    //Ends  Removing the Previous Created Calender



    //Locate the Input Location First
    const element = document.getElementById(OpenCalenderPosition);
    const DateInputXY = element.getBoundingClientRect();
    var leftPosnInputWindow = parseInt(DateInputXY.left.toFixed()) + 5; //Gives left edge distance from ViewPort
    var rightPosnInputWindow = parseInt(DateInputXY.right.toFixed()) + 5;
    var scrollTop = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
    var bottomPosnInputDoc = parseInt(DateInputXY.bottom.toFixed()) + parseInt(scrollTop) + 2;// Gives Bottom Edge Distance from --Document---
    var topPosnInputWindow = parseInt(DateInputXY.top.toFixed());
    const viewPortHeight = window.innerHeight;
    const viewPortWidth = parseInt(window.innerWidth);
    //Location Ends

    //Create Div To Push Calender
    const body = document.getElementsByTagName("BODY")[0];
    const displayCalenderDiv = document.createElement('div');
    displayCalenderDiv.setAttribute('class', 'DisplayCalenderDiv');
    displayCalenderDiv.setAttribute('id', CurrentInput + "UT");
    displayCalenderDiv.appendChild(Calender);
    body.appendChild(displayCalenderDiv);
    //Locate the location Of Input

    var calenderHeight = document.getElementById(CurrentInput + 'UT').clientHeight;
    var calenderWidth = document.getElementById(CurrentInput + 'UT').clientWidth;


    //IF WindowHEIGHT-InputTop > calenderheight then opencalender at bottom of input.
    if ((viewPortHeight - parseInt(DateInputXY.bottom.toFixed())) >= calenderHeight) {
        document.getElementById(CurrentInput + 'UT').style.top = bottomPosnInputDoc + "px";
    }
    //Top Opening Position Of calender  = Scroll + WindowTopOfInput -HeightofCalender
    else {
        document.getElementById(CurrentInput + 'UT').style.top = (parseInt(scrollTop) + topPosnInputWindow - calenderHeight - 2) + "px";
    }
    // If following true then open calender on the right edge

    // Deciding THe Position Of Calender to Open Left
    if ((viewPortWidth - parseInt(DateInputXY.left.toFixed())) >= calenderWidth) {
        document.getElementById(CurrentInput + 'UT').style.left = leftPosnInputWindow + "px";
    }
    //Top Opening Position Of calender  = Scroll + WindowTopOfInput -HeightofCalender
    else {
        document.getElementById(CurrentInput + 'UT').style.right = viewPortWidth - parseInt(rightPosnInputWindow) + "px";
    }

    document.getElementsByTagName('body')[0].setAttribute('onmouseup', "RemoveCalenderCheck(event ,'" + CurrentInput + "')");
}
//Closes all the Open Calender Div
function CloseCalender(Obj) {
    var SingleCalender = Obj.StartDateInput;
    var RangeCalender = Obj.RangeInput;
    // Removing the Previous Created Calender
    if (RangeCalender != null && RangeCalender != "") {
        var previousStartCalender = document.getElementById(RangeCalender.StartDateInput + "UT");
        if (previousStartCalender != null) {
            previousStartCalender.remove();
        }
        var previousEndCalender = document.getElementById(RangeCalender.EndDateInput + "UT");
        if (previousEndCalender != null) {
            previousEndCalender.remove();
        }

    }
    else if (SingleCalender != null && SingleCalender != "") {
        var previouscalender = document.getElementById(SingleCalender + "UT");
        if (previouscalender != null) {
            previouscalender.remove();
        }
    }

}

function RemoveCalenderCheck(event, Id) {
    var calender = document.getElementById(Id + "UT");

    if (!calender.contains(event.target)) {
        document.body.removeAttribute('onmouseup');
        calender.style.display = 'none';
        setTimeout(function () { calender.remove() }, 1000);

    }

}

function ClearCalenderDates(Obj) {

    CloseCalender(Obj);

    var SingleCalender = Obj.StartDateInput;
    var RangeCalender = Obj.RangeInput;
    // Removing the Previous Created Calender
    if (RangeCalender != null && RangeCalender != "") {

        Obj.StartDate = null;
        Obj.EndDate = null;
        Obj.SelectedStartDate = null;
        Obj.SelectedEndDate = null;
        document.getElementById(RangeCalender.StartDateInput).value = null;
        document.getElementById(RangeCalender.EndDateInput).value = null;
        document.getElementById(RangeCalender.StartDateInput).click();

    }
    else if (SingleCalender != null && SingleCalender != "") {
        document.getElementById(SingleCalender).value = null;
        document.getElementById(SingleCalender).click();
    }
}

function SelectStartOrEnd(Obj, Select) {
    var RangeCalender = Obj.RangeInput;
    // Removing the Previous Created Calender
    if (RangeCalender != null && RangeCalender != "") {
        if (Select == 'Start') {
            document.getElementById(RangeCalender.StartDateInput).click();
        }
        else {
            document.getElementById(RangeCalender.EndDateInput).click();
        }
    }
}

//Arrow Button On the Date Calender Uses This Function
function DateChangeUT(Year, Month, Obj) {
    var GetDate = new Date(Year, Month);
    Obj.StartYear = GetDate.getFullYear();
    Obj.StartMonth = GetDate.getMonth();
    Obj.DateArrowActive = 'Y';
    DateCalender(Obj);
}
//Ends

//When we select the date in textbox or other it will hide the Calender after selecting
function DateSelect(YearInt, MonthInt, DateInt, Obj) {
    //Object Update with Current Date
    Obj.StartMonth = MonthInt;
    Obj.StartYear = YearInt;
    //Closes Calender On Date Selection

    var CurrentSelectedInput = Obj.CurrentSelectedInput;
    var CallBackFunc = Obj.CallBackFunc;
    var StartDateInput = Obj.StartDateInput;
    var RangeInput = Obj.RangeInput;
    var EndDateInput = Obj.EndDateInput;


    //Checking If Range Calender then Update Obj.SelectedStartDate = null; Obj.SelectedEndDate = null;for highlight;
    if (RangeInput != null && RangeInput != "") {
        if (RangeInput.StartDateInput == CurrentSelectedInput) {
            Obj.SelectedStartDate = new Date(YearInt, MonthInt, DateInt);
        }
        if (RangeInput.EndDateInput == CurrentSelectedInput) {
            Obj.SelectedEndDate = new Date(YearInt, MonthInt, DateInt);
        }
    }
    //
    var previouscalender = document.getElementById(CurrentSelectedInput + "UT");
    if (previouscalender != null) {
        previouscalender.remove();
    }
    //End
    var selectedDate = new Date(YearInt, MonthInt, DateInt);
    var DatePickerElement = document.getElementById(CurrentSelectedInput);

    // Default Format
    var month = 'MMM';
    var year = 'YYYY';
    var format = 'D/MMM/YYYY';

    /// Check the Passed Format in the input
    var passedFormat = Obj.DateFormat;
    var passedMonth = passedFormat.match(/M/gi).length;
    switch (passedMonth) {
        case 1: month = 'M';
            break;
        case 2: month = 'MM';
            break;
        case 3: month = 'MMM';
            break;
    }

    var passedYear = passedFormat.match(/Y/gi).length;
    switch (passedYear) {
        case 2: year = 'YY';
            break;
        case 4: year = 'YYYY';
            break;
    }

    if (passedFormat != null) { format = passedFormat; }
    //In these variable we save the date to be put in textbox
    var returnDate;
    var returnMonth;
    var returnYear;
    //Ends
    switch (month) {
        case 'M':
            returnMonth = selectedDate.getMonth();
            break;
        case 'MM':

            returnMonth = selectedDate.toLocaleString('en-US', { month: 'short' });
            break;
        default:
            returnMonth = selectedDate.toLocaleString('en-US', { month: 'long' });
            break;
    }

    switch (year) {
        case "YY": returnYear = selectedDate.getFullYear().toString().slice(-2);
            break;

        default: returnYear = selectedDate.getFullYear();
            break;
    }

    switch (format) {
        case 'D-M-YYYY':
        case 'D-MM-YYYY':
        case 'D-MMM-YYYY':
        case 'D-M-YY':
        case 'D-MM-YY':
        case 'D-MMM-YY': returnDate = DateInt + '-' + returnMonth + '-' + returnYear;
            break;
        case 'M-D-YYYY':
        case 'MM-D-YYYY':
        case 'MMM-D-YYYY':
        case 'M-D-YY':
        case 'MM-D-YY':
        case 'MMM-D-YY': returnDate = returnMonth + '-' + DateInt + '-' + returnYear;
            break;
        case 'M-D-YYYY':
        case 'MM-D-YYYY':
        case 'MMM-D-YYYY':
        case 'M-D-YY':
        case 'MM-D-YY':
        case 'MMM-D-YY': returnDate = returnYear + '-' + returnMonth + '-' + DateInt;
            break;
        default: returnDate = returnYear + '-' + returnMonth + '-' + DateInt;
            break;
    }

    DatePickerElement.value = returnDate;
    // If the Range Calender then

    document.body.removeAttribute('onmouseup');
    var ClickedCalender = document.getElementById(CurrentSelectedInput + "UT");
    if (ClickedCalender != null && ClickedCalender != "") {
        ClickedCalender.style.display = 'none';
        setTimeout(function () { ClickedCalender.remove() }, 1000);
    }

    // If Range Calender and Start date is selected then Fire Click Event on the EndDate Input
    if (RangeInput != null) {
        if (CurrentSelectedInput == StartDateInput) {
            //Check If the Date Start Date>= EndDate then Empty End Date
            var StartDate = new Date(document.getElementById(StartDateInput).value);
            var EndDate = new Date(document.getElementById(EndDateInput).value);
            if (StartDate >= EndDate) {
                document.getElementById(EndDateInput).value = "";
                Obj.SelectedEndDate = null;
            }
            document.getElementById(EndDateInput).click();
            Obj.CurrentSelectedInput = EndDateInput;

        }
    }


    if (CallBackFunc != null && CallBackFunc != "undefined" && CallBackFunc != "") {
        CallBackFunc();
    }
}
//Ends
//Ends:-DatePicker
//MaxMin the Div
function MaxMin() {
    var maxdivs = document.getElementsByClassName('MaxMinDv');
    for (var i = 0; i < maxdivs.length; i++) {
        maxdivs[i].classList.add('PnRe');
        var divid = 'PtDnDv' + i;
        maxdivs[i].setAttribute('id', divid);
        var btndv = document.createElement('div');
        btndv.classList.add('MaxMinBtn');
        btndv.classList.add('MaxContent');
        btndv.setAttribute('btndvid', divid);
        btndv.addEventListener("click", function (ev) {
            var element = document.getElementById(ev.target.getAttribute('btndvid'));
            if (element.classList.contains("MaximizeDv")) {
                element.parentElement.classList.remove('MaxMinPopUpBackground');
                element.classList.remove("MaximizeDv");
                btndv.classList.remove('MinContent');
                btndv.classList.add('MaxContent');
            }
            else {
                element.parentElement.classList.add('MaxMinPopUpBackground');
                element.classList.add("MaximizeDv");
                btndv.classList.remove('MaxContent');
                btndv.classList.add('MinContent');
            }
        });
        maxdivs[i].appendChild(btndv);
    }
}
//Ends

//DropDownlist select data on the bases of text
function DnTextSelect(DnIdStg, TtStg) {
    var DD = document.getElementById(DnIdStg);
    for (var i = 0; i < DD.length; i++) {
        if (DD.options[i].text === TtStg) {
            DD.selectedIndex = i;
            break;
        }
    }
}
//End

function YearDpDn() {
    //Reference the DropDownList.
    var ddlYears = document.getElementById("ddlYears");
    //Determine the Current Year.
    var currentYear = (new Date()).getFullYear();
    //Loop and add the Year values to DropDownList.
    for (var i = 2020; i <= currentYear; i++) {
        var option = document.createElement("OPTION");
        if (i == "2020") {
            option.innerHTML = "Select Year";
            option.value = "";
            ddlYears.appendChild(option);
        }
        else {
            option.innerHTML = i;
            option.value = i;
            ddlYears.appendChild(option);
        }
    }
}

//stop Scroll in type=number input
document.addEventListener("wheel", function (event) {
    if (document.activeElement.type === "number") {
        document.activeElement.blur();
    }
});
//Ends

function HidePopUpScript() {
    document.getElementById('PopUpScript').style.display = "none";
}

//Hide the PopUp
function HideValidationPopUp() {
    document.getElementById('ValPopUp').style.display = "none";
}
//Ends

function Empty(IdStg) {
    document.getElementById(IdStg).value = "";
}


//Show the Current User Clipped Data Count in the Layout
function UrCdDa() {
    getUT('/CommonMethods/ClippedDataCount', function (data) {
        document.getElementById('UrCdDa').innerHTML = data;
    });
}
//Ends


//Create div to show User Mapped Company
function UrCyDpDn() {
    getUT("/CommonMethods/UserCompanies", function (data) {
        StOnCy = document.getElementById('CompDivID');
        document.getElementById('CompDivID').innerHTML = "";
        var json = JSON.parse(data);
        if (json.length == 1) {
            StOnCyS = document.getElementById('CCyNe');
            document.getElementById('CCyNe').innerHTML = "";
            var obj2 = json[0];
            const AddCompanySpan = document.createElement('span');
            AddCompanySpan.setAttribute('id', obj2.Id);
            AddCompanySpan.setAttribute('onclick', "ChangeCompany('" + obj2.Id + "')");
            AddCompanySpan.setAttribute('complogo', obj2.Images);
            StOnCyS.appendChild(AddCompanySpan);
            AddCompanySpan.innerHTML = obj2.Name;
        }
        else {
            for (var i = 0; i < json.length; i++) {
                var obj = json[i];
                const AddCompanyDiv = document.createElement('div');
                AddCompanyDiv.setAttribute('id', obj.Id);
                AddCompanyDiv.setAttribute('onclick', "ChangeCompany('" + obj.Id + "')");
                AddCompanyDiv.setAttribute('complogo', obj.Images);
                StOnCy.appendChild(AddCompanyDiv);
                AddCompanyDiv.innerHTML = '<i class="UT-CompanyBuilding" >&nbsp;' + obj.Name;
            }
        }
    })
}
//Ends

function GetExcel(tableId, File) {
    let tableData = document.getElementById(tableId).outerHTML;
    //tableData = tableData.replace(/<A[^>]*>|<\/A>/g, ""); //remove if u want links in your table
    tableData = tableData.replace(/<input[^>]*>|<\/input>/gi, ""); //remove input params
    tableData = tableData;

    let a = document.createElement('a');
    a.href = `data:application/vnd.ms-excel, ${encodeURIComponent(tableData)}`
    a.download = 'UT ' + File + '.xls'
    a.click()
}

function ShowHide(IDSg) {

    var element = document.getElementById(IDSg);
    if (element.style.display === "none") {
        element.style.display = "block";
    }
    else {
        element.style.display = "none";
    }
}

function Show(IDSg) {
    document.getElementById(IDSg).style.display = "block";
}
function Hide(IDSg) {
    document.getElementById(IDSg).style.display = "none";
}


function CopyToClipboard(ID) {
    if (document.selection) {
        var range = document.body.createTextRange();
        range.moveToElementText(document.getElementById(ID));
        range.select().createTextRange();
        document.execCommand("copy");
    } else if (window.getSelection) {
        var range = document.createRange();
        range.selectNode(document.getElementById(ID));
        window.getSelection().addRange(range);
        document.execCommand("copy");
    }
}
//CharacterCountInput
function CrCtIt(IdStg, MsgId, MaxLength) {
    document.getElementById(MsgId).innerHTML = "&nbsp Left : " + (MaxLength - parseInt(document.getElementById(IdStg).value.length)) + "&nbsp";
}

function FormFocus(FormID) {
    var form = document.getElementById(FormID);
    var elementC = parseInt(form.elements.length);
    for (var i = 0; i < elementC; i++) {
        var e = form.elements[i];
        if (e.getAttribute('type') !== 'hidden' && e.tagName === 'INPUT' || e.tagName === 'SELECT') {
            e.focus();
            break;
        }
    }
}

function FormReset(FormID) {
    document.getElementById(FormID).reset();
}

function ShowPassword(InputIDStg, IconShowID, IconHideId) {
    var iconshow = document.getElementById(IconShowID);
    var iconhide = document.getElementById(IconHideId);
    var x = document.getElementById(InputIDStg);
    if (document.getElementById(InputIDStg).type === "password") {
        x.type = "text";
        iconhide.style.display = "block";
        iconshow.style.display = "none";
    } else {
        x.type = "password";
        iconhide.style.display = "none";
        iconshow.style.display = "block";
    }
}


function HidePopUp(IdStg) {
    var ID = "PopUp"
    if (IdStg != null) {
        ID = IdStg;
    }
    document.getElementById(ID).style.display = "none";
}
function HidePopUpTop() {
    document.getElementById("PopUpTop").style.display = "none";
}

function ShowPopUp() {
    document.getElementById("PopUp").style.display = "block";
}

function ShowLoader() {
    document.getElementById("loader").style.display = "flex";
}

function HideLoader() {
    document.getElementById("loader").style.display = "none";
}

function HidePopUp() {
    document.getElementById("PopUp").style.display = "none";
}

function PopUpAutoSucess() {
    var popup = document.getElementById("PopUp");
    popup.style.width = "40%";
    popup.style.left = "30%";
    document.getElementById("PopUpErrorID").style.display = "none";
    document.getElementById("PopUpDetailID").style.display = "none";
    document.getElementById("PopUpSuccessID").style.display = "block";
    popup.style.display = "block";
    HideLoader();
    var myTimeout = setTimeout(HidePopUp, 800);
}

function PopUpError(data) {
    var popup = document.getElementById("PopUp");
    popup.style.width = "70%";
    popup.style.left = "15%";
    document.getElementById("PopUpErrorContentID").innerHTML = data;
    document.getElementById("PopUpSuccessID").style.display = "none";
    document.getElementById("PopUpDetailID").style.display = "none";
    document.getElementById("PopUpErrorID").style.display = "block";
    popup.style.display = "block";
    HideLoader();

}

function PopUpUpdate(data) {
    var popup = document.getElementById("PopUp");
    popup.style.width = "70%";
    popup.style.left = "15%";
    document.getElementById("PopUpDetailID").style.display = "block";
    document.getElementById("PopUpDetailContentID").innerHTML = data;
    document.getElementById("PopUpErrorID").style.display = "none";
    document.getElementById("PopUpSuccessID").style.display = "none";
    document.getElementById("PopUp").style.display = "block";
    HideLoader();
}

function PopUpForm(data) {
    var popup = document.getElementById("PopUp");
    popup.style.width = "90%";
    popup.style.left = "5%";
    document.getElementById("PopUpDetailID").style.display = "block";
    document.getElementById("PopUpDetailContentID").innerHTML = data;
    document.getElementById("PopUpErrorID").style.display = "none";
    document.getElementById("PopUpSuccessID").style.display = "none";
    document.getElementById("PopUp").style.display = "block";
    HideLoader();
}


function FormCollectionUT(FormID) {
    var Form = document.getElementById(FormID);
    const FD = new FormData();
    // Push data
    for (const [name, value] of Form) {
        FD.append(name, value);
    }
    return FD;
}

function postUT(URL, CallbackFunc, PostData, FormID) {
    var request = new XMLHttpRequest();
    if (CallbackFunc != null) {
        request.onload = function () {
            CallbackFunc(this.responseText, request.status, "", FormID);
        };
    }
    request.open("POST", URL);
    request.send(PostData);
}


function getUT(URL, CallbackFunc) {
    var request = new XMLHttpRequest();
    request.open('GET', URL);
    request.onload = function () {
        CallbackFunc(this.responseText, request.status);
    }
    request.send();
}



function FormSubmit(PostURL, FormID, CallFunc, FormName) {
    ShowLoader();
    var form = document.getElementById(FormID);
    // Bind the FormData object and the form element
    var FD = new FormData(form);
    try {
        var inputfield = 'form[name='.concat(FormName, "] ", 'input[type="file"][multiple]');
        const docs = document.querySelector(inputfield);
        if (docs.files.length > 0) {
            for (let i = 0; i < docs.files.length; i++) {
                FD.append(`docs_${i}`, docs.files[i]);
            }
        }
    } catch { }
    postUT(PostURL, CallFunc, FD, FormID);
}

//There
// This Function Runs On Error and Success both because error response is also treated as success by HttpRequest
function FormAjaxSuccess(data, FormID, FormFieldID, UpdateType, UpdateDivID, FormFieldURL, RedirectURL) {
    var UpdateDiv = "Def";
    if (UpdateDivID != null) { UpdateDiv = UpdateDivID; }
    // If Text includes resposnseformUT it means Form Submitted Sucessfully else there is error
    if (data.includes("responseformUT")) {
        // case if validation error then show to user
        if (data.includes("ShowValidation")) {
            document.getElementById(FormFieldID).innerHTML = "";
            document.getElementById(FormFieldID).innerHTML = data;
            HideLoader();
        }
        // Case if Everthing Successfull then update according 3cases as follow
        //1 Update Nothing just Show Success Message & form Reset
        //2 Update Side or Some html div and show Success Message & Form Reset
        //3 Update Side or Some html div and show Success Message & Form Not Reset & Call Further Function
        //4 Update on Success Message Pop Itself & Form Reset
        //5 Update Side or Some html div and show Success Message & LHS form View
        //6 Update Nothing just Show Success Message & form Reset & Call Further Function
        else {
            if (data.includes("SuccessPop")) {
                switch (UpdateType) {
                    case "UpdateNothing": PopUpAutoSucess(); FormReset(FormID); FormFocus(FormID); break;

                    case "UpdateDiv":
                        PopUpAutoSucess();
                        document.getElementById(UpdateDiv).innerHTML = "";
                        document.getElementById(UpdateDiv).insertAdjacentHTML('afterbegin', data);
                        FormReset(FormID);
                        FormFocus(FormID);
                        break;

                    case "UpdateDivNoFReset":
                        PopUpAutoSucess();
                        HideLoader();
                        document.getElementById(UpdateDiv).innerHTML = "";
                        document.getElementById(UpdateDiv).insertAdjacentHTML('afterbegin', data);
                        break;

                    case "UpdatePopUp":
                        PopUpUpdate(data);
                        FormReset(FormID);
                        FormFocus(FormID);
                        break;

                    case "UpdateEditDiv":
                        PopUpAutoSucess();
                        document.getElementById(UpdateDiv).innerHTML = "";
                        document.getElementById(UpdateDiv).innerHTML = data;
                        //Refresh the LHS View(ANkita)
                        var xhrget = new XMLHttpRequest();
                        xhrget.open('GET', FormFieldURL, true);
                        xhrget.onload = function () {
                            document.getElementById(FormFieldID).innerHTML = "";
                            document.getElementById(FormFieldID).innerHTML = xhrget.responseText;
                            FormFocus(FormID);
                        };
                        xhrget.send(null);
                        //Ends
                        break;

                    case "UpdateNothingNextFun":
                        FormReset(FormID);
                        UpdateNextDiv();
                        break;
                }
            }
            else {
                window.location.href = RedirectURL;
            }
        }
    }
    else {
        PopUpError(data);//Go in this Function and show the Error Message PopUp
    }
}

function FormAjaxFail(xhr, status, error, FormID) {
    PopUpError(error);
}



//Tabs Script
function TabsUT(TabLinkIdStg, TabActiveInt, TabDisableAryInt, TabHideAryInt) {
    var TablinkId = document.getElementById(TabLinkIdStg);
    TablinkId.classList.add("UTabs");
    var tablink = TablinkId.children;
    var tabcontent = [];
    for (var i = 0; i < tablink.length; i++) {

        //Adds TabContent
        var getContentTab = document.getElementById(TabLinkIdStg + "Cont" + i);
        tabcontent.push(getContentTab);
        //
        //Adds Id To Link 
        tablink[i].setAttribute("id", TabLinkIdStg + [i]);
        tablink[i].setAttribute("Num", [i]);
        tablink[i].setAttribute('onclick', "ClickTabUT('" + TabLinkIdStg + "','" + TabLinkIdStg + [i] + "')");
        //Ends
        //Check Which Tab To be Activate At Start
        tablink[i].classList.add('UTabLink');
        tabcontent[i].classList.add('UTabCont');
        //Hide All TabContent First
        tabcontent[i].style.display = 'none'


        if (i === 0) {
            tablink[i].classList.add('UTabActive');
            tabcontent[i].style.display = 'block'
        }
        if (TabActiveInt === i) {
            tablink[0].classList.remove('UTabActive');
            tablink[i].classList.add('UTabActive');
            tabcontent[0].style.display = 'none';
            tabcontent[i].style.display = 'block';
        }
        //Ends
    }
    try {
        DisableTabUt(TabLinkIdStg, TabDisableAryInt);
    } catch { }
    try {
        HideTabUt(TabLinkIdStg, TabHideAryInt)
    } catch { }
}

function ClickTabUT(TabLinkIdStg, ClickedTabIdStg) {
    var tablink = document.getElementById(TabLinkIdStg).children;
    var tabcontent = [];
    for (var i = 0; i < tablink.length; i++) {

        //Adds TabContent
        var getContentTab = document.getElementById(TabLinkIdStg + "Cont" + i);
        tabcontent.push(getContentTab);
    }
    for (var i = 0; i < tablink.length; i++) {
        try { tabcontent[i].style.display = 'none'; }
        catch { }
        try {
            tablink[i].classList.remove("UTabActive");
        }
        catch { }
    }
    var clickedtab = document.getElementById(ClickedTabIdStg);
    clickedtab.classList.add("UTabActive");
    tabcontent[clickedtab.getAttribute("Num")].style.display = "block";
}

function DisableTabUt(TabLinkIdStg, TabDisableInt) {
    for (var i = 0; i < TabDisableInt.length; i++) {
        var tabId = TabLinkIdStg + TabDisableInt[i];
        var tab = document.getElementById(tabId);
        tab.removeAttribute("onclick");
        document.getElementById(TabLinkIdStg + "Cont" + TabDisableInt[i]).style.display = "none";

        if (tab.classList.contains("UTabActive")) {
            tab.classList.remove('UTabActive');
        }
        tab.classList.add('UTabDeactive');
    }
}

function HideTabUt(TabLinkIdStg, TabHideAryInt) {
    for (var i = 0; i < TabHideAryInt.length; i++) {
        var tabId = TabLinkIdStg + TabHideAryInt[i];
        var tab = document.getElementById(tabId);
        document.getElementById(TabLinkIdStg + "Cont" + TabHideAryInt[i]).style.display = "none";
        tab.style.display = "none";
        if (tab.classList.contains("UTabActive")) {
            tab.classList.remove('UTabActive');
        }
    }
}


function ActiveTabUt(TabLinkIdStg, TabActiveInt) {
    var TablinkId = document.getElementById(TabLinkIdStg);
    TablinkId.classList.add("UTabs");
    var tablink = TablinkId.children;
    tablink[TabActiveInt].setAttribute('onclick', "ClickTabUT('" + TabLinkIdStg + "','" + TabLinkIdStg + [TabActiveInt] + "')");

    var tabcontent = [];
    for (var i = 0; i < tablink.length; i++) {

        //Adds TabContent
        var getContentTab = document.getElementById(TabLinkIdStg + "Cont" + i);
        tabcontent.push(getContentTab);

        //Hide All TabContent First
        tabcontent[i].style.display = 'none'
        if (tablink[i].classList.contains('UTabActive')) {
            tablink[i].classList.remove('UTabActive');
        }

        if (TabActiveInt === i) {
            if (tablink[i].classList.contains('UTabDeactive')) {
                tablink[i].classList.remove('UTabDeactive');
            }
            tablink[i].classList.add('UTabActive');
            tablink[i].style.display = 'flex';
            tabcontent[i].style.display = 'block';
        }

    }
}
//Ends




//Tabs Script
function UTabHz(TabLinkIdStg, TabContentIdStg, TabActiveInt, TabDisableAryInt, TabHideAryInt) {
    var TablinkId = document.getElementById(TabLinkIdStg);
    TablinkId.classList.add("UTabs");
    var tablink = TablinkId.children;
    var TabContentId = document.getElementById(TabContentIdStg);
    TabContentId.classList.add('UTabsContent')
    var tabcontent = TabContentId.children;
    for (var i = 0; i < tablink.length; i++) {
        //Adds Id To Link and Content divs
        tablink[i].setAttribute("id", TabLinkIdStg + [i]);
        tablink[i].setAttribute("Num", [i]);
        tabcontent[i].setAttribute("id", TabContentIdStg + [i] + "Cont");

        //Ends
        //Check Which Tab To be Activate At Start
        tablink[i].classList.add('UTabLink');
        tabcontent[i].classList.add('UTabCont');

        if (i === 0) {
            tablink[i].classList.add('UTabActive');
            tabcontent[i].style.display = 'block'
        }
        if (TabActiveInt === i) {
            tablink[0].classList.remove('UTabActive');
            tablink[i].classList.add('UTabActive');
            tabcontent[0].style.display = 'none';
            tabcontent[i].style.display = 'block';
        }
        //Ends
        //Active the Tabs
        tablink[i].addEventListener('click', event => {

            for (var i = 0; i < tablink.length; i++) {

                try { tabcontent[i].style.display = 'none'; }
                catch { }

                try {
                    tablink[i].classList.remove("UTabActive");
                }
                catch { }
            }
            var clickedtab = document.getElementById(event.srcElement.id);
            clickedtab.classList.add("UTabActive");
            tabcontent[clickedtab.getAttribute("Num")].style.display = "block";
        })
        //Ends
    }
}
//Ends

//Start:- DatePicker
//function ShowCalender(InputIdStg, CalenderTypeStg, StartYearInt, StartMonthStg) {

//    var CalenderType = 'Date';

//    var StartYear = new Date().getFullYear();
//    var StartMonth = new Date().getMonth();
//    if (CalenderTypeStg != null) {
//        CalenderType = CalenderTypeStg;
//    }


//    if (StartMonthStg != null) {
//        StartMonth = StartMonthStg;
//    }


//    if (StartYearInt != null) {
//        StartYear = StartYearInt;
//    }

//    switch (CalenderType) {
//        case 'Date': DateCalender(StartYear, StartMonth, InputIdStg);
//            break;
//        case 'Year': YearCalender(StartYear, InputIdStg);
//            break;
//        case 'Month': MonthCalender(StartYear, InputIdStg);
//            break;
//    }



//}

//function DateCalender(UserYearInt, UserMonthInt, DatePickerIdStg, UserLangStg) {

//    //Gets the current Date variables
//    var today = new Date();

//    var UserYear = today.getFullYear();
//    if (UserYearInt != null) { UserYear = UserYearInt; }

//    var UserMonth = today.getMonth();
//    if (UserMonthInt != null) { UserMonth = UserMonthInt; }

//    var todayDate = UserYear + '-' + UserMonth + '-' + today.getDate();
//    var UserLang = 'en-US';
//    if (UserLangStg != null) { UserLang = UserLangStg; }

//    var WeekDay = ['Mo', 'Tu', 'We', 'Th', 'Fr', 'Sa', 'Su'];
//    var month = new Date(UserYear, UserMonth).toLocaleString(UserLang, { month: 'short' });
//    var monthNum = UserMonth;
//    var year = UserYear;
//    var MonthDays = new Date(UserYear, UserMonth + 1, 0).getDate();


//    function GetDayName(dateStr) {
//        var date = new Date(dateStr);
//        return date.toLocaleDateString(UserLang, { weekday: 'long' });
//    }



//    const CalenderDiv = document.createElement("div");
//    CalenderDiv.setAttribute('class', 'CalenderDiv');
//    CalenderDiv.setAttribute('id', 'UTCalenderDateDiv');


//    //Creating the Calender Header with Icon and Current Month & Year
//    //Main Head Div
//    const CalenderHeadDiv = document.createElement('div');
//    CalenderHeadDiv.setAttribute('class', 'CalenderHead CalenderRowHighlight');
//    CalenderDiv.appendChild(CalenderHeadDiv);
//    //Main Head child Add
//    const CalenderIconMonthDiv = document.createElement('div');
//    CalenderIconMonthDiv.setAttribute('class', 'CalenderUpDownIcon');
//    CalenderHeadDiv.appendChild(CalenderIconMonthDiv);

//    const AddMonthDiv = document.createElement('div');
//    AddMonthDiv.setAttribute('onclick', "DateChangeUT(" + parseInt(UserYear) + "," + parseInt(UserMonth + 1) + ", '" + DatePickerIdStg + "' )");
//    AddMonthDiv.setAttribute('class', "Cr-uPr");
//    CalenderIconMonthDiv.appendChild(AddMonthDiv);
//    AddMonthDiv.innerHTML = '<i class="UT-PlusCircleSolid" >';

//    const SubMonthDiv = document.createElement('div');
//    SubMonthDiv.setAttribute('onclick', "DateChangeUT(" + parseInt(UserYear) + "," + parseInt(UserMonth - 1) + ", '" + DatePickerIdStg + "' )");
//    SubMonthDiv.setAttribute('class', "Cr-uPr");
//    CalenderIconMonthDiv.appendChild(SubMonthDiv);
//    SubMonthDiv.innerHTML = '<i class="UT-MinusCircleSolid" > </i>';


//    const CalenderCurrentMonth = document.createElement('div');
//    CalenderCurrentMonth.setAttribute('onclick', "MonthCalender(" + parseInt(UserYear) + ", '" + DatePickerIdStg + "' )");
//    CalenderCurrentMonth.setAttribute('class', 'Cr-uPr');
//    CalenderCurrentMonth.innerHTML = month + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
//    CalenderHeadDiv.appendChild(CalenderCurrentMonth);

//    const CalenderCurrentYear = document.createElement('div');
//    CalenderCurrentYear.setAttribute('onclick', "YearCalender(" + parseInt(UserYear) + ", '" + DatePickerIdStg + "' )");
//    CalenderCurrentYear.setAttribute('class', 'Cr-uPr');
//    //Checks Leap Year
//    if (new Date(year, 1, 29).getDate() === 29) {
//        CalenderCurrentYear.innerHTML = year + '&nbsp' + 'L' + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
//    }
//    else {
//        CalenderCurrentYear.innerHTML = year + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
//    }
//    CalenderHeadDiv.appendChild(CalenderCurrentYear);

//    const CalenderIconYearDiv = document.createElement('div');
//    CalenderIconYearDiv.setAttribute('class', 'CalenderUpDownIcon');
//    CalenderHeadDiv.appendChild(CalenderIconYearDiv);

//    const AddYearDiv = document.createElement('div');
//    AddYearDiv.setAttribute('onclick', "DateChangeUT(" + parseInt(UserYear + 1) + "," + parseInt(UserMonth) + ", '" + DatePickerIdStg + "' )");
//    AddYearDiv.setAttribute('class', "Cr-uPr");
//    CalenderIconYearDiv.appendChild(AddYearDiv);
//    AddYearDiv.innerHTML = '<i class="UT-PlusCircleSolid" > </i>';

//    const SubYearDiv = document.createElement('div');
//    SubYearDiv.setAttribute('onclick', "DateChangeUT(" + parseInt(UserYear - 1) + "," + parseInt(UserMonth) + ", '" + DatePickerIdStg + "' )");
//    SubYearDiv.setAttribute('class', "Cr-uPr");
//    CalenderIconYearDiv.appendChild(SubYearDiv);
//    SubYearDiv.innerHTML = '<i class="UT-MinusCircleSolid" > </i>';


//    //Main Head Ends

//    //Create DayNameRow
//    const CalenderWeekDayDiv = document.createElement('div');
//    CalenderWeekDayDiv.setAttribute('class', 'CalenderDayName CalenderRowHighlight');
//    CalenderDiv.appendChild(CalenderWeekDayDiv);

//    for (var i = 0; i < WeekDay.length; i++) {
//        const DayName = document.createElement('div');
//        DayName.innerHTML = WeekDay[i];
//        CalenderWeekDayDiv.appendChild(DayName);
//    }

//    // Adding Calender Rows
//    var monthDate = 1;
//    var monthStartDayName = GetDayName(new Date(year, UserMonth, monthDate));
//    //For Comparing Month Start Date DayName
//    var WeekDayName = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday'];

//    for (var i = 0; i < 6; i++) {
//        var CalenderRowDiv = document.createElement('div');
//        CalenderRowDiv.setAttribute('class', 'CalenderDayRow');

//        for (var weekDay = 0; weekDay < 7; weekDay++) {
//            var CalenderRowDayDiv = document.createElement('div');

//            if (monthStartDayName == WeekDayName[weekDay] && monthDate <= MonthDays) {
//                CalenderRowDayDiv.innerHTML = monthDate;
//                var passdate = new Date(year, monthNum, monthDate);
//                CalenderRowDayDiv.setAttribute('onclick', "DateSelect( " + UserYear + "," + UserMonth + "," + monthDate + " , '" + DatePickerIdStg + "' )");
//                //Active Day Highlight + ", '" + DatePickerIdStg + "' )");
//                var activedate = today.getFullYear() + '-' + today.getMonth() + '-' + monthDate;

//                if (activedate == todayDate) {

//                    CalenderRowDayDiv.setAttribute('class', "CalenderDayActiveUT");

//                }

//                monthDate++;
//                monthStartDayName = GetDayName(new Date(year, monthNum, monthDate));
//            }
//            else {
//                CalenderRowDayDiv.innerHTML = " ";
//            }
//            CalenderRowDiv.appendChild(CalenderRowDayDiv);


//        }
//        CalenderDiv.appendChild(CalenderRowDiv);

//        // Month Date exceed the total number of days in the month break loop
//        if (monthDate > MonthDays) {
//            break;
//        }
//    }

//    AddCalender(CalenderDiv, DatePickerIdStg);
//}

//function MonthCalender(YearInt, DatePickerIdStg) {
//    CYear = new Date().getFullYear();
//    if (YearInt == null) {
//        YearInt = CYear;
//    }

//    const CalenderDiv = document.createElement("div");
//    CalenderDiv.setAttribute('class', 'CalenderMonthDiv');
//    CalenderDiv.setAttribute('id', 'UTCalenderMonthDiv');


//    //Creating the Calender Header with Icon and Current Month & YearInt
//    //Main Head Div
//    const CalenderHeadDiv = document.createElement('div');
//    CalenderHeadDiv.setAttribute('class', 'CalenderMonthHead CalenderRowHighlight');
//    CalenderDiv.appendChild(CalenderHeadDiv);
//    //Main Head child Add
//    const CalenderIconMonthDiv = document.createElement('div');
//    CalenderHeadDiv.appendChild(CalenderIconMonthDiv);
//    CalenderIconMonthDiv.innerHTML = '<i class="UT-MonthNumberLine FtSz18"></i>' + ' &nbsp; ' + ' Months'

//    const CalenderCurrentYear = document.createElement('div');
//    CalenderCurrentYear.setAttribute('onclick', "YearCalender(" + parseInt(YearInt) + ", '" + DatePickerIdStg + "' )");
//    CalenderCurrentYear.setAttribute('class', 'Cr-uPr');
//    //Checks Leap Year
//    if (new Date(YearInt, 1, 29).getDate() === 29) {
//        CalenderCurrentYear.innerHTML = YearInt + '&nbsp' + 'L' + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
//    }
//    else {
//        CalenderCurrentYear.innerHTML = YearInt + '&nbsp' + '<i class="UT-CursorHandLine"></i>';
//    }
//    CalenderHeadDiv.appendChild(CalenderCurrentYear);


//    //Add Months
//    var MonthName = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];

//    var monthindex = 0;

//    for (var r = 0; r < 3; r++) {

//        const CalenderMonthDiv = document.createElement('div');
//        CalenderMonthDiv.setAttribute('class', 'CalenderMonthRow');
//        CalenderDiv.appendChild(CalenderMonthDiv);

//        for (var i = 0; i < 4; i++) {
//            const MonthInitials = document.createElement('div');
//            if (MonthName[monthindex] == new Date().toLocaleString('en-US', { month: 'short' })) {
//                MonthInitials.setAttribute('class', 'CalenderDayActiveUT');
//            }

//            MonthInitials.innerHTML = MonthName[monthindex];
//            MonthInitials.setAttribute('onclick', "DateCalender(" + parseInt(YearInt) + "," + parseInt(monthindex) + "," + "'" + DatePickerIdStg + "'" + ")");

//            CalenderMonthDiv.appendChild(MonthInitials);
//            monthindex++;
//        }
//    }
//    AddCalender(CalenderDiv, DatePickerIdStg);
//}

//function YearCalender(YearInt, DatePickerIdStg) {

//    CYear = new Date().getFullYear();
//    if (YearInt == null) {
//        YearInt = CYear;
//    }
//    const CalenderDiv = document.createElement("div");
//    CalenderDiv.setAttribute('class', 'CalenderDiv');
//    CalenderDiv.setAttribute('id', 'UTCalenderYearDiv');


//    //Creating the Calender Header with Icon and Current Month & YearInt
//    //Main Head Div
//    var CalenderHeadDiv = document.createElement('div');
//    CalenderHeadDiv.setAttribute('class', 'CalenderHead CalenderRowHighlight');
//    CalenderDiv.appendChild(CalenderHeadDiv);
//    //Main Head child Add
//    var CalenderIconYearDiv = document.createElement('div');
//    CalenderIconYearDiv.setAttribute('class', 'CalenderUpDownIcon');
//    CalenderHeadDiv.appendChild(CalenderIconYearDiv);

//    var YearJump3 = document.createElement('div');
//    YearJump3.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt + 75) + ", '" + DatePickerIdStg + "' )");
//    YearJump3.setAttribute('class', "Cr-uPr");
//    CalenderIconYearDiv.appendChild(YearJump3);
//    YearJump3.innerHTML = '<i class="UT-ArrowUpThreeLineCircleSolid" >';

//    var YearJump2 = document.createElement('div');
//    YearJump2.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt + 50) + ", '" + DatePickerIdStg + "' )");
//    YearJump2.setAttribute('class', "Cr-uPr");
//    CalenderIconYearDiv.appendChild(YearJump2);
//    YearJump2.innerHTML = '<i class="UT-ArrowUpTwoLineCircleSolid" > </i>';

//    var YearJump1 = document.createElement('div');
//    YearJump1.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt + 25) + ", '" + DatePickerIdStg + "' )");
//    YearJump1.setAttribute('class', "Cr-uPr");
//    CalenderIconYearDiv.appendChild(YearJump1);
//    YearJump1.innerHTML = '<i class="UT-ArrowUpOneLineCircleSolid" > </i>';


//    const CalenderYears = document.createElement('div');
//    CalenderYears.innerHTML = '<i class="UT-PlusCircleSolid" ></i>.Years.<i class="UT-MinusCircleSolid"></i>';
//    CalenderHeadDiv.appendChild(CalenderYears);

//    var CalenderIconDownYearDiv = document.createElement('div');
//    CalenderIconDownYearDiv.setAttribute('class', 'CalenderUpDownIcon');
//    CalenderHeadDiv.appendChild(CalenderIconDownYearDiv);

//    var YearJumpDn3 = document.createElement('div');
//    YearJumpDn3.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt - 25) + ", '" + DatePickerIdStg + "' )");
//    YearJumpDn3.setAttribute('class', "Cr-uPr");
//    CalenderIconDownYearDiv.appendChild(YearJumpDn3);
//    YearJumpDn3.innerHTML = '<i class="UT-ArrowDownOneLineCircleSolid" >';

//    var YearJumpDn2 = document.createElement('div');
//    YearJumpDn2.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt - 50) + ", '" + DatePickerIdStg + "' )");
//    YearJumpDn2.setAttribute('class', "Cr-uPr");
//    CalenderIconDownYearDiv.appendChild(YearJumpDn2);
//    YearJumpDn2.innerHTML = '<i class="UT-ArrowDownTwoLineCircleSolid" > </i>';

//    var YearJumpDn1 = document.createElement('div');
//    YearJumpDn1.setAttribute('onclick', "YearSetChangeUT(" + parseInt(YearInt - 75) + ", '" + DatePickerIdStg + "' )");
//    YearJumpDn1.setAttribute('class', "Cr-uPr");
//    CalenderIconDownYearDiv.appendChild(YearJumpDn1);
//    YearJumpDn1.innerHTML = '<i class="UT-ArrowDownThreeLineCircleSolid" > </i>';
//    //Main Head Ends
//    var yearindex = 0;
//    for (var r = 0; r < 5; r++) {
//        const CalenderYearRowDiv = document.createElement('div');
//        CalenderYearRowDiv.setAttribute('class', 'CalenderYearRow');
//        CalenderDiv.appendChild(CalenderYearRowDiv);

//        for (var i = 0; i < 5; i++) {
//            var yearUd = YearInt + yearindex;
//            const YearInit = document.createElement('div');
//            YearInit.innerHTML = yearUd;
//            if (yearUd == new Date().getFullYear()) { YearInit.setAttribute('class', 'CalenderDayActiveUT') }
//            YearInit.setAttribute('onclick', "MonthCalender(" + parseInt(YearInt + yearindex) + ", '" + DatePickerIdStg + "' )");
//            CalenderYearRowDiv.appendChild(YearInit);
//            yearindex++;
//        }
//    }

//    AddCalender(CalenderDiv, DatePickerIdStg);


//}

////Adds Calender To UI
//function AddCalender(Calender, InputId) {
//    var previouscalender = document.getElementById(InputId + "UT");
//    if (previouscalender != null) {
//        previouscalender.remove();
//    }

//    //Locate the Input Location First
//    const element = document.getElementById(InputId);
//    const DateInputXY = element.getBoundingClientRect();
//    var leftPosnInputWindow = parseInt(DateInputXY.left.toFixed()) + 5; //Gives left edge distance from ViewPort
//    var scrollTop = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
//    var bottomPosnInputDoc = parseInt(DateInputXY.bottom.toFixed()) + parseInt(scrollTop) + 2;// Gives Bottom Edge Distance from --Document---
//    var topPosnInputWindow = parseInt(DateInputXY.top.toFixed());
//    const viewPortHeight = window.innerHeight;
//    //Location Ends

//    //Create Div To Push Calender
//    const body = document.getElementsByTagName("BODY")[0];
//    const displayCalenderDiv = document.createElement('div');
//    displayCalenderDiv.setAttribute('class', 'DisplayCalenderDiv');
//    displayCalenderDiv.setAttribute('id', InputId + "UT");
//    displayCalenderDiv.appendChild(Calender);
//    body.appendChild(displayCalenderDiv);
//    //Locate the location Of Input

//    var calenderHeight = document.getElementById(InputId + 'UT').clientHeight;

//    //IF WindowHEIGHT-InputTop > calenderheight then opencalender at bottom of input.
//    if ((viewPortHeight - parseInt(DateInputXY.bottom.toFixed())) >= calenderHeight) {
//        document.getElementById(InputId + 'UT').style.top = bottomPosnInputDoc + "px";
//        document.getElementById(InputId + 'UT').style.left = leftPosnInputWindow + "px";
//    }
//    //Top Opening Position Of calender  = Scroll + WindowTopOfInput -HeightofCalender
//    else {
//        document.getElementById(InputId + 'UT').style.top = (parseInt(scrollTop) + topPosnInputWindow - calenderHeight - 2) + "px";
//        document.getElementById(InputId + 'UT').style.left = leftPosnInputWindow + "px";
//    }

//    //Hide the DatePicker when we click outside the datepicker or input(Ankita)
//    document.addEventListener('mouseup', function (e) {
//        var previouscalender = document.getElementById(InputId + "UT");
//        if (previouscalender != null) {
//            previouscalender.style.display = 'none';
//        }
//    });
//    //Ends(Ankita)
//}
////Closes all the Open Calender Div

//function CloseCalenderDiv() {
//    var yeardiv = document.getElementById('UTCalenderYearDiv');
//    if (yeardiv != null) {
//        yeardiv.remove();
//    }

//    var monthdiv = document.getElementById('UTCalenderMonthDiv');
//    if (monthdiv != null) {
//        monthdiv.remove();
//    }
//    var Datediv = document.getElementById('UTCalenderDateDiv');
//    if (Datediv != null) {
//        Datediv.remove();
//    }
//}

////Year Calender Arrow Buttons Call This To Shuffle Year Set
//function YearSetChangeUT(Year, DatePickerIdStg) {
//    YearCalender(Year, DatePickerIdStg);
//}
////On Year Select This Shows Month Calender

////Arrow Button On the Date Calender Uses This Function
//function DateChangeUT(Year, Month, DatePickerIdStg) {
//    var GetDate = new Date(Year, Month);
//    DateCalender(GetDate.getFullYear(), GetDate.getMonth(), DatePickerIdStg);
//}
////Ends

////When we select the date in textbox or other it will hide the Calender after selecting
//function DateSelect(YearInt, MonthInt, DateInt, DatePickerIdStg) {
//    //Closes Calender On Date Selection
//    var previouscalender = document.getElementById(DatePickerIdStg + "UT");
//    if (previouscalender != null) {
//        previouscalender.remove();
//    }
//    //End
//    var selectedDate = new Date(YearInt, MonthInt, DateInt);
//    var DatePickerElement = document.getElementById(DatePickerIdStg);

//    // Default Format 
//    var month = 'MMM';
//    var year = 'YYYY';
//    var format = 'D/MMM/YYYY';

//    /// Check the Passed Format in the input
//    var passedFormat = DatePickerElement.getAttribute('UTDate');
//    var passedMonth = passedFormat.match(/M/gi).length;
//    switch (passedMonth) {
//        case 1: month = 'M';
//            break;
//        case 2: month = 'MM';
//            break;
//        case 3: month = 'MMM';
//            break;
//    }

//    var passedYear = passedFormat.match(/Y/gi).length;
//    switch (passedYear) {
//        case 2: year = 'YY';
//            break;
//        case 4: year = 'YYYY';
//            break;
//    }

//    if (passedFormat != null) { format = passedFormat; }

//    //In these variable we save the date to be put in textbox
//    var returnDate;
//    var returnMonth;
//    var returnYear;
//    //Ends


//    switch (month) {
//        case 'M':
//            returnMonth = selectedDate.getMonth();
//            break;
//        case 'MM':

//            returnMonth = selectedDate.toLocaleString('en-US', { month: 'short' });
//            break;
//        default:
//            returnMonth = selectedDate.toLocaleString('en-US', { month: 'long' });
//            break;
//    }

//    switch (year) {
//        case "YY": returnYear = selectedDate.getFullYear().toString().slice(-2);
//            break;

//        default: returnYear = selectedDate.getFullYear();
//            break;
//    }

//    switch (format) {

//        case 'D-M-YYYY':
//        case 'D-MM-YYYY':
//        case 'D-MMM-YYYY':
//        case 'D-M-YY':
//        case 'D-MM-YY':
//        case 'D-MMM-YY': returnDate = DateInt + '-' + returnMonth + '-' + returnYear;
//            break;


//        case 'M-D-YYYY':
//        case 'MM-D-YYYY':
//        case 'MMM-D-YYYY':
//        case 'M-D-YY':
//        case 'MM-D-YY':
//        case 'MMM-D-YY': returnDate = returnMonth + '-' + DateInt + '-' + returnYear;
//            break;


//        case 'M-D-YYYY':
//        case 'MM-D-YYYY':
//        case 'MMM-D-YYYY':
//        case 'M-D-YY':
//        case 'MM-D-YY':
//        case 'MMM-D-YY': returnDate = returnYear + '-' + returnMonth + '-' + DateInt;
//            break;


//        default: returnDate = returnYear + '-' + returnMonth + '-' + DateInt;
//            break;
//    }
//    DatePickerElement.value = returnDate;
//}
////Ends
////Ends:-DatePicker

//Ankita
//Show the Detail View
function Details(URL) {
    var loader = document.getElementById("loader");

    var GetMethod = new XMLHttpRequest();
    loader.style.display = "flex";
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        PopUpUpdate(content);
    };
    GetMethod.send(null);
}
//Ends

//Check and UnCheck the all CheckBoxs in the Index View
function AmSelectEntries() {
    var checkbox = document.getElementsByClassName("AmIndexCheckbox");
    if (document.getElementById("AmIndexCheckBoxThId").checked == true) {
        var i = 0;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = "checked";
        }
    }
    else {
        var i = 0;
        for (var i = 0; i < checkbox.length; i++) {
            checkbox[i].checked = false;
        }
    }
}
//Ends


//Refresh the Index List according to the pages(Amit Sir and Ankita)
function PaginationPageNoBtn(PgSelected, UrlPagination, UrlIndex, Element) {
    var SearchForm = Element.getAttribute("SearchFormID");
    var SearchType = Element.getAttribute("SearchType");
    switch (SearchType) {
        case "DateSearch":
            var form = document.getElementById(SearchForm);
            // Bind the FormData object and the form element
            var pagesize = document.getElementById("TablePageSizeID").value;
            var FD = new FormData(form);
            FD.append("PageNoSelected", PgSelected);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, DateSuccessFn, FD, SearchForm);
            break;
        case "TableSearch":
            var form = document.getElementById(SearchForm);
            var pagesize = document.getElementById("TablePageSizeID").value;
            var FD = new FormData(form);
            FD.append("PageNoSelected", PgSelected);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, TableSuccessFn, FD, SearchForm);
            break;
        case "FilterSearch":
            var form = document.getElementById(SearchForm);
            var pagesize = document.getElementById("TablePageSizeID").value;
            var FD = new FormData(form);
            FD.append("PageNoSelected", PgSelected);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, FilterSuccessFn, FD, SearchForm);
            break;
        case "NoSearch":
            var pagesize = document.getElementById("TablePageSizeID").value;
            var URLIndex = UrlIndex + '?PgSelectedNum=' + PgSelected + '&PgSize=' + pagesize + '&PageCall=Show';
            getUT(URLIndex, D);
            function D(data) {
                FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                var doc = new DOMParser().parseFromString(data, "text/html");
                var pageinfo = doc.getElementById("PaginationAt");
                var netrecord = pageinfo.getAttribute("netrecord");
                var pageno = pageinfo.getAttribute("pagenumber");
                var pagesize = pageinfo.getAttribute("PageSize");

                RefreshPagination(UrlPagination + '?PgSelectedNum=' + PgSelected + '&PgSize=' + pagesize + '&SearchType=NoSearch&NetRecords=' + netrecord);
            }
            break;
    }
    document.getElementById("loader").style.display = "flex";
}
//Ends

//How many enteries show in one time
function PaginationPageSize(UrlPagination, UrlIndex, Element) {
    var SearchForm = Element.getAttribute("SearchFormID");
    var SearchType = Element.getAttribute("SearchType");
    var pagesize = document.getElementById("TablePageSizeID").value;
    switch (SearchType) {
        case "DateSearch":
            var form = document.getElementById(SearchForm);
            // Bind the FormData object and the form element
            var FD = new FormData(form);
            FD.append("PageNoSelected", 1);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, DateSuccessFn, FD, SearchForm);
            break;
        case "TableSearch":
            var form = document.getElementById(SearchForm);
            var FD = new FormData(form);
            FD.append("PageNoSelected", 1);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, TableSuccessFn, FD, SearchForm);
            break;
        case "FilterSearch":
            var form = document.getElementById(SearchForm);
            var FD = new FormData(form);
            FD.append("PageNoSelected", 1);
            FD.append("PageSize", pagesize);
            postUT(UrlIndex, FilterSuccessFn, FD, SearchForm);
            break;
        case "NoSearch":
            var URLIndex = UrlIndex + '?PgSelectedNum=' + 1 + '&PgSize=' + pagesize + '&PageCall=Show';
            getUT(URLIndex, D);
            function D(data) {
                FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                var doc = new DOMParser().parseFromString(data, "text/html");
                var pageinfo = doc.getElementById("PaginationAt");
                var netrecord = pageinfo.getAttribute("netrecord");
                var pageno = pageinfo.getAttribute("pagenumber");
                var pagesize = pageinfo.getAttribute("PageSize");

                RefreshPagination(UrlPagination + '?PgSelectedNum=' + 1 + '&PgSize=' + pagesize + '&SearchType=NoSearch&NetRecords=' + netrecord);
            }
            break;
    }
    document.getElementById("loader").style.display = "flex";
}
//Ends

//Refresh and Show the Pagination Div
function RefreshPagination(URL) {
    //Get Method Start
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        //Refresh the Index table
        document.getElementById("PagenationShow").innerHTML = "";
        document.getElementById("PagenationShow").innerHTML = GetMethod.responseText;
        //Ends
    };
    GetMethod.send(null);
}

//Show the PopUp of Surity to Delete Data
function Delete(DeleteURL, ListShowURL) {
    document.getElementById("PopUpTop").style.display = "block";//Show the PopUp
    document.getElementById("DeleteUrl").value = DeleteURL;//Store the URL of the DeleteComfirmed Action with Id
    document.getElementById("ListUrl").innerHTML = ListShowURL;//Store the URL of the List Action
}
//Ends

//Go to the Verifydata action and Verify,Unverify,active and inactive acc to btn call(Ankita)
function VerifyData(VUrl, listurl) {
    //Post Method Start
    var xhrget = new XMLHttpRequest();
    xhrget.open('POST', VUrl, true);
    xhrget.onload = function () {
        if (xhrget.responseText.includes("SuccessPop")) {
            HidePopUpTop();//Hide the TopPopUp
            HidePopUp();//Hide the Main PopUp
            document.getElementById("loader").style.display = "none";//Hide the Loader
            PopUpAutoSucess();
            //Refresh the Pagination Div
            var PgNo = document.getElementById("PgNum").innerHTML;
            var PgSize = document.getElementById("TablePageSizeID").value;
            var SearchForm = document.getElementById("SearchFormID").innerHTML;
            var SearchType = document.getElementById("SearchType").innerHTML;
            switch (SearchType) {
                case "DateSearch":
                    var form = document.getElementById(SearchForm);
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, DateSuccessFn, FD, SearchForm);
                    break;
                case "TableSearch":
                    var form = document.getElementById(SearchForm);
                    var pagesize = document.getElementById("TablePageSizeID").value;
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, TableSuccessFn, FD, SearchForm);
                    break;
                case "FilterSearch":
                    var form = document.getElementById(SearchForm);
                    var pagesize = document.getElementById("TablePageSizeID").value;
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, FilterSuccessFn, FD, SearchForm);
                    break;
                case "NoSearch":
                    var URLIndex = listurl + '/Index' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&PageCall=Show';
                    getUT(URLIndex, Done);
                    function Done(data) {
                        FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                        var doc = new DOMParser().parseFromString(data, "text/html");
                        var pageinfo = doc.getElementById("PaginationAt");
                        var netrecord = pageinfo.getAttribute("netrecord");
                        var pageno = pageinfo.getAttribute("pagenumber");
                        var pagesize = pageinfo.getAttribute("PageSize");
                        RefreshPagination(listurl + '/Pagination' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&SearchType=NoSearch&NetRecords=' + netrecord);
                    }
                    break;
                    break;
            }
            //Ends

        }
        if (xhrget.responseText.includes("Error")) {
            HidePopUpTop();//Hide the TopPopUp
            HidePopUp();//Hide the Main PopUp

            PopUpUpdate(xhrget.responseText);
        }
    };
    xhrget.send(null);
    //Ends
}
//Ends

//Go to the DeleteConfirmed Action to delete the data
function DeleteConfirmed() {
    document.getElementById("loader").style.display = "flex";//Show the Loader
    var deleteurl = document.getElementById("DeleteUrl").value;//Pick the Stored URL of the DeleteComfirmed Action with Id
    var listurl = document.getElementById("ListUrl").innerHTML;//Pick the Stored URL of the Index Action
    //Post Method Start
    var xhrget = new XMLHttpRequest();
    xhrget.open('POST', deleteurl, true);
    xhrget.onload = function () {
        if (xhrget.responseText.includes("SuccessPop")) {
            HidePopUpTop();//Hide the TopPopUp
            HidePopUp();//Hide the Main PopUp
            document.getElementById("loader").style.display = "none";//Hide the Loader
            //PopUpAutoSucess();
            //Refresh the Pagination Div
            var PgNo = document.getElementById("PgNum").innerHTML;
            var PgSize = document.getElementById("TablePageSizeID").value;
            var SearchForm = document.getElementById("SearchFormID").innerHTML;
            var SearchType = document.getElementById("SearchType").innerHTML;
            switch (SearchType) {
                case "DateSearch":
                    var form = document.getElementById(SearchForm);
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, DateSuccessFn, FD, SearchForm);
                    break;
                case "TableSearch":
                    var form = document.getElementById(SearchForm);
                    var pagesize = document.getElementById("TablePageSizeID").value;
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, TableSuccessFn, FD, SearchForm);
                    break;
                case "FilterSearch":
                    var form = document.getElementById(SearchForm);
                    var pagesize = document.getElementById("TablePageSizeID").value;
                    var FD = new FormData(form);
                    FD.append("PageNoSelected", PgNo);
                    FD.append("PageSize", PgSize);
                    var URL = listurl + "/" + SearchType + "/";
                    postUT(URL, FilterSuccessFn, FD, SearchForm);
                    break;
                case "NoSearch":
                    var URLIndex = listurl + '/Index' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&PageCall=Show';
                    getUT(URLIndex, Done);
                    function Done(data) {
                        FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                        var doc = new DOMParser().parseFromString(data, "text/html");
                        var pageinfo = doc.getElementById("PaginationAt");
                        var netrecord = pageinfo.getAttribute("netrecord");
                        var pageno = pageinfo.getAttribute("pagenumber");
                        var pagesize = pageinfo.getAttribute("PageSize");
                        RefreshPagination(listurl + '/Pagination' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&SearchType=NoSearch&NetRecords=' + netrecord);
                    }
                    break;
                    break;
            }
            //Ends

        }
        if (xhrget.responseText.includes("Error")) {
            HidePopUpTop();//Hide the TopPopUp
            HidePopUp();//Hide the Main PopUp

            PopUpUpdate(xhrget.responseText);
        }
    };
    xhrget.send(null);
    //Ends
}
//Ends

function AddSearchGroup() {

    var getItemGroup = document.getElementsByClassName('SformFields');

    var groupCount = parseInt(document.getElementById("searchGroupCntUT").value) + 1;
    document.getElementById("searchGroupCntUT").value = groupCount;

    var getItem = getItemGroup[0];
    var copyItem = getItem.cloneNode(true);
    var newId = "sform-group" + groupCount;
    copyItem.id = newId;
    document.getElementById("sform-group").after(copyItem);

    //HereWeUpdateTheClonedElementNameAndId..?

    var newFormGroup = document.getElementById(newId);

    UpdateFormGroup(newFormGroup, groupCount);
}

// Updates the New FormGroup With Id and FieldsName
function UpdateFormGroup(newFormGroup, groupCount) {

    var Name = newFormGroup.children["Name"];
    var Type = newFormGroup.children["Type"];
    var TextSearch = newFormGroup.children["TextSearch"];
    var IntSearchFrom = newFormGroup.children["IntSearchFrom"];
    var IntSearchTo = newFormGroup.children["IntSearchTo"];
    var DateSearchFrom = newFormGroup.children["DateSearchFrom"];
    var DateSearchTo = newFormGroup.children["DateSearchTo"];
    var BoolSearchTrue = newFormGroup.children["BoolSearchTrue"];
    var BoolSearchFalse = newFormGroup.children["BoolSearchFalse"];
    var ShowTrue = newFormGroup.children["ShowTrue"];
    var ShowFalse = newFormGroup.children["ShowFalse"];
    var DelButton = newFormGroup.children["DelButton"];
    var SelectCol = newFormGroup.children["selectList"];

    NameID = "Name" + groupCount;
    TypeID = "Type" + groupCount;
    TextSearchID = "TextSearch" + groupCount;
    IntSearchFromID = "IntSearchFrom" + groupCount;
    IntSearchToID = "IntSearchTo" + groupCount;
    DateSearchFromID = "DateSearchFrom" + groupCount;
    DateSearchToID = "DateSearchTo" + groupCount;
    BoolSearchTrueID = "BoolSearchTrue" + groupCount;
    BoolSearchFalseID = "BoolSearchFalse" + groupCount;
    ShowTrueID = "ShowTrue" + groupCount;
    ShowFalseID = "ShowFalse" + groupCount;
    SelectColID = "selectList" + groupCount;


    Name.id = NameID;
    Type.id = TypeID;
    TextSearch.id = TextSearchID;
    IntSearchFrom.id = IntSearchFromID;
    IntSearchTo.id = IntSearchToID;
    DateSearchFrom.id = DateSearchFromID;
    DateSearchTo.id = DateSearchToID;
    BoolSearchTrue.id = BoolSearchTrueID;
    BoolSearchFalse.id = BoolSearchFalseID;
    ShowTrue.id = ShowTrueID;
    ShowFalse.id = ShowFalseID;
    SelectCol.id = SelectColID;
    DelButton.id = groupCount;


    document.getElementById(NameID).setAttribute('name', 'IndexSearchList[' + groupCount + '].Name');

    document.getElementById(TypeID).setAttribute('name', 'IndexSearchList[' + groupCount + '].Type');
    document.getElementById(TextSearchID).setAttribute('name', 'IndexSearchList[' + groupCount + '].TextSearch');
    document.getElementById(IntSearchFromID).setAttribute('name', 'IndexSearchList[' + groupCount + '].IntSearchFrom');
    document.getElementById(IntSearchToID).setAttribute('name', 'IndexSearchList[' + groupCount + '].IntSearchTo');
    document.getElementById(DateSearchFromID).setAttribute('name', 'IndexSearchList[' + groupCount + '].DateSearchFrom');
    document.getElementById(DateSearchToID).setAttribute('name', 'IndexSearchList[' + groupCount + '].DateSearchTo');
    document.getElementById(BoolSearchTrueID).setAttribute('name', 'IndexSearchList[' + groupCount + '].BoolSearch');
    document.getElementById(BoolSearchFalseID).setAttribute('name', 'IndexSearchList[' + groupCount + '].BoolSearch');

    document.getElementById(SelectColID).setAttribute('name', 'selectList' + groupCount);
    document.getElementById(SelectColID).setAttribute('count', groupCount);

    //dates();
}

function HideShowGroupFields(allfield) {
    var showField = allfield.value;
    var FgroupId = allfield.name;
    var column = allfield.options[allfield.selectedIndex].text;
    var count = allfield.getAttribute("count");

    var Name = "Name" + count;
    var Type = "Type" + count;
    var TextSearchID = "TextSearch" + count;
    var IntSearchFromID = "IntSearchFrom" + count;
    var IntSearchToID = "IntSearchTo" + count;
    var DateSearchFromID = "DateSearchFrom" + count;
    var DateSearchToID = "DateSearchTo" + count;
    var BoolSearchTrueID = "BoolSearchTrue" + count;
    var BoolSearchFalseID = "BoolSearchFalse" + count;
    var ShowTrueID = "ShowTrue" + count;
    var ShowFalseID = "ShowFalse" + count;


    document.getElementById(TextSearchID).style.display = "none";
    document.getElementById(IntSearchFromID).style.display = "none";
    document.getElementById(IntSearchToID).style.display = "none";
    document.getElementById(DateSearchFromID).style.display = "none";
    document.getElementById(DateSearchToID).style.display = "none";
    document.getElementById(BoolSearchTrueID).style.display = "none";
    document.getElementById(BoolSearchFalseID).style.display = "none";
    document.getElementById(ShowTrueID).style.display = "none";
    document.getElementById(ShowFalseID).style.display = "none";

    //string,int,date,bool

    switch (showField) {
        case "string":
            document.getElementById(TextSearchID).style.display = "block";

            break;
        case "int":
            document.getElementById(IntSearchFromID).style.display = "block";
            document.getElementById(IntSearchToID).style.display = "block";
            break;

        case "DateTime":
            document.getElementById(DateSearchFromID).style.display = "block";
            document.getElementById(DateSearchToID).style.display = "block";
            break;

        case "bool":
            document.getElementById(ShowTrueID).style.display = "block";
            document.getElementById(ShowFalseID).style.display = "block";
            document.getElementById(BoolSearchTrueID).style.display = "block";
            document.getElementById(BoolSearchFalseID).style.display = "block";
            break;
        default:
        // code block
    }

    document.getElementById(Name).value = column;
    document.getElementById(Type).value = showField;

}

function DeleteGroup(id) {
    var newId = "sform-group" + id;
    document.getElementById(newId).remove();
}

//Function for Star and Unstar Mark in Jquery
//Pending Convert to Jquery to Javascript
function MultiButtonFunc(CallMethod, ID, display, ActionURL) {
    var PgNo = document.getElementById("PgNum").innerHTML;
    var PgSize = document.getElementById("TablePageSizeID").value;
    ShowLoader();
    var data = [];
    var token = $("input[name='__RequestVerificationToken']").val();
    if (ID === "") {
        //This Is Applied For the CheckBox and Here We Check If the Check Box Marked Are Star Marked Or Not

        $(".AmIndexCheckbox").each(function () {
            if ($(this).prop('checked')) {
                if (CallMethod === 'Star') {
                    if ($(this).attr('ClipStatus').toString() === 'UnSelected') {
                        data.push({
                            key: $(this).val().toString(),
                            value: $(this).attr('name').toString()
                        });
                    }
                }

                else {
                    if ($(this).attr('ClipStatus').toString() === 'Selected') {
                        data.push($(this).val());
                    }
                }

            }
        });

    }
    else {
        if (CallMethod === 'Star') {
            data.push({
                key: ID,
                value: display
            });
        }
        else {
            data.push(ID);
        }
    }

    if (data.length > 0) {
        if (CallMethod === 'Star') {

            $.post(ActionURL + '/MarkStar/', { ID: data, PageNo: PgNo, PageSize: PgSize }, function (ddata) {
                UrCdDa();//Update the Clipped Data value in the Layout
                var SearchForm = document.getElementById("SearchFormID").innerHTML;
                var SearchType = document.getElementById("SearchType").innerHTML;
                switch (SearchType) {
                    case "DateSearch":
                        var form = document.getElementById(SearchForm);
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, DateSuccessFn, FD, SearchForm);
                        break;
                    case "TableSearch":
                        var form = document.getElementById(SearchForm);
                        var pagesize = document.getElementById("TablePageSizeID").value;
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, TableSuccessFn, FD, SearchForm);
                        break;
                    case "FilterSearch":
                        var form = document.getElementById(SearchForm);
                        var pagesize = document.getElementById("TablePageSizeID").value;
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, FilterSuccessFn, FD, SearchForm);
                        break;
                    case "NoSearch":
                        var URLIndex = ActionURL + '/Index' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&PageCall=Show';
                        getUT(URLIndex, Done);
                        function Done(data) {
                            FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                            var doc = new DOMParser().parseFromString(data, "text/html");
                            var pageinfo = doc.getElementById("PaginationAt");
                            var netrecord = pageinfo.getAttribute("netrecord");
                            var pageno = pageinfo.getAttribute("pagenumber");
                            var pagesize = pageinfo.getAttribute("PageSize");
                            RefreshPagination(ActionURL + '/Pagination' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&SearchType=NoSearch&NetRecords=' + netrecord);
                        }
                        break;
                        break;
                }
            });
        }
        if (CallMethod === 'UnStar') {
            $.post(ActionURL + '/UnMarkStar/', { ID: data, PageNo: PgNo, PageSize: PgSize }, function (d) {
                UrCdDa();//Update the Clipped Data value in the Layout
                var SearchForm = document.getElementById("SearchFormID").innerHTML;
                var SearchType = document.getElementById("SearchType").innerHTML;
                switch (SearchType) {
                    case "DateSearch":
                        var form = document.getElementById(SearchForm);
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, DateSuccessFn, FD, SearchForm);
                        break;
                    case "TableSearch":
                        var form = document.getElementById(SearchForm);
                        var pagesize = document.getElementById("TablePageSizeID").value;
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, TableSuccessFn, FD, SearchForm);
                        break;
                    case "FilterSearch":
                        var form = document.getElementById(SearchForm);
                        var pagesize = document.getElementById("TablePageSizeID").value;
                        var FD = new FormData(form);
                        FD.append("PageNoSelected", PgNo);
                        FD.append("PageSize", PgSize);
                        var URL = ActionURL + "/" + SearchType + "/";
                        postUT(URL, FilterSuccessFn, FD, SearchForm);
                        break;
                    case "NoSearch":
                        var URLIndex = ActionURL + '/Index' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&PageCall=Show';
                        getUT(URLIndex, Done);
                        function Done(data) {
                            FormAjaxSuccess(data, '', '', "UpdateDivNoFReset", "Def");

                            var doc = new DOMParser().parseFromString(data, "text/html");
                            var pageinfo = doc.getElementById("PaginationAt");
                            var netrecord = pageinfo.getAttribute("netrecord");
                            var pageno = pageinfo.getAttribute("pagenumber");
                            var pagesize = pageinfo.getAttribute("PageSize");
                            RefreshPagination(ActionURL + '/Pagination' + '?PgSelectedNum=' + PgNo + '&PgSize=' + PgSize + '&SearchType=NoSearch&NetRecords=' + netrecord);
                        }
                        break;
                        break;
                }
            });
        }
    }


}

//Show the Password change popup by User inside the Application
function ShowChangePassForm(UserEmailId) {
    var URL = "/Account/ChangePassword?UserEmailId=" + UserEmailId;
    ShowHide('UserDivID');
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        document.getElementById("PopUpDetailID").style.display = "block";
        document.getElementById("PopUpDetailContentID").innerHTML = "";
        document.getElementById("PopUpDetailContentID").innerHTML = content;
        document.getElementById("PopUpErrorID").style.display = "none";
        document.getElementById("PopUpSuccessID").style.display = "none";
        document.getElementById("PopUp").style.display = "block";
        HideLoader();
    };
    GetMethod.send(null);
}

//Success function of the Password Change
function ChangePassSuccess(data, status, xhr, FormID) {
    FormAjaxSuccess(data, FormID, '', "UpdatePopUp");
    document.getElementById("PopUp").style.display = "none";
    document.getElementById("PopUpDetailContentID").innerHTML = "";
    document.getElementById("PopUpDetailID").style.display = "none";
}

//Show the Password enter Popup before changing the Email id by the User in Inside the Application
function ShowPassVerifyForm(UserEmailId) {
    var URL = "/Account/PasswordVerify?UserEmailId=" + UserEmailId;
    ShowHide('UserDivID');
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        document.getElementById("PopUpDetailID").style.display = "block";
        document.getElementById("PopUpDetailContentID").innerHTML = "";
        document.getElementById("PopUpDetailContentID").innerHTML = content;
        document.getElementById("PopUpErrorID").style.display = "none";
        document.getElementById("PopUpSuccessID").style.display = "none";
        document.getElementById("PopUp").style.display = "block";
        HideLoader();
    };
    GetMethod.send(null);
}

//Success Function of the Password entry
function PassVSuccess(data, status, xhr, FormID, FormFieldID) {
    FormAjaxSuccess(data, FormID, FormFieldID, "UpdateNothingNextFun");
}

//After Password Entry successfully here pick the user id and go to the ShowEmailChange Function
function UpdateNextDiv() {
    var userid = document.getElementById("EmailIdForChange").innerHTML;
    ShowEmailChangeForm(userid);
}

//Show the Email Change form Popup changed by the User inside the application
function ShowEmailChangeForm(UserId) {
    var URL = "/Account/ChangeEmail?UserId=" + UserId;
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        document.getElementById("PopUpDetailID").style.display = "block";
        document.getElementById("PopUpDetailContentID").innerHTML = "";
        document.getElementById("PopUpDetailContentID").innerHTML = content;
        document.getElementById("PopUpErrorID").style.display = "none";
        document.getElementById("PopUpSuccessID").style.display = "none";
        document.getElementById("PopUp").style.display = "block";
        HideLoader();
    };
    GetMethod.send(null);
}

//Success function of the Email Change
function ChangeESuccess(data, status, xhr, FormID) {
    FormAjaxSuccess(data, FormID, '', "UpdatePopUp");
    document.getElementById("PopUp").style.display = "none";
    document.getElementById("PopUpDetailContentID").innerHTML = "";
    document.getElementById("PopUpDetailID").style.display = "none";
}

//Show the All Clipped Data of the Current User in the PopUp
function ShowClippedData() {
    var URL = "/CommonMethods/UserAllClippedData";
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        var popup = document.getElementById("PopUp");
        popup.style.width = "70%";
        popup.style.left = "15%";
        document.getElementById("PopUpSuccessID").style.display = "none";
        document.getElementById("PopUpErrorID").style.display = "none";
        document.getElementById("PopUpDetailID").style.display = "block";
        document.getElementById("PopUpDetailContentID").innerHTML = "";
        document.getElementById("PopUpDetailContentID").innerHTML = content;
        document.getElementById("PopUp").style.display = "block";
        HideLoader();
    };
    GetMethod.send(null);
}

//To Add the Remark of Particular Clipped Data
function EditClippedData(Id) {
    var URL = "/CommonMethods/EditClippedData?Id=" + Id;
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('GET', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        document.getElementById("PopUpDetailID").style.display = "block";
        document.getElementById("PopUpDetailContentID").innerHTML = "";
        document.getElementById("PopUpDetailContentID").innerHTML = content;
        document.getElementById("PopUpErrorID").style.display = "none";
        document.getElementById("PopUpSuccessID").style.display = "none";
        document.getElementById("PopUp").style.display = "block";
        HideLoader();
    };
    GetMethod.send(null);
}

//Success function of the Edit Clipped Data
function SuccessEditClippedData(data, status, xhr, FormID, FormFieldID) {
    FormAjaxSuccess(data, FormID, '', "Update");
    ShowClippedData();
}

//Delete the Particular clipped data
function DeleteClippedData(Id) {
    var URL = "/CommonMethods/DeleteClippedData?id=" + Id;
    ShowLoader();
    var GetMethod = new XMLHttpRequest();
    GetMethod.open('POST', URL, true);
    GetMethod.onload = function () {
        var content = GetMethod.responseText;
        //document.getElementById("PopUpDetailID").style.display = "block";
        //document.getElementById("PopUpDetailContentID").innerHTML = "";
        //document.getElementById("PopUpDetailContentID").innerHTML = content;
        //document.getElementById("PopUpErrorID").style.display = "none";
        //document.getElementById("PopUpSuccessID").style.display = "none";
        //document.getElementById("PopUp").style.display = "block";
        UrCdDa();
        HideLoader();
        ShowClippedData();
    };
    GetMethod.send(null);
}
//Ends Ankita


///////////////////AutoCompelete STart////////////////////////////////


function StartSuggestionBox(MinCharInt, InputIDStg, URL, ExtFieldName, ExtFieldID, CallBack) {
    var input = document.getElementById(InputIDStg);
    document.getElementById(InputIDStg).addEventListener('keydown', function (e) {
        if (e.which === 9) {
            SuggestionClose(InputIDStg);
        }
    });

    document.getElementById(InputIDStg).addEventListener('keyup', function (e) {
        e.preventDefault();
        KeyPress(input.value, MinCharInt, InputIDStg, e.which, URL, ExtFieldName, ExtFieldID, CallBack);
    }, false);

}

function KeyPress(InputValStg, MinCharInt, InputIDStg, KeyCode, URL, ExtFieldName, ExtFieldID, CallBack) {
    switch (KeyCode) {
        case 38:
        case 40:
            //MoveDown();
            SuggestionNavigate(InputIDStg + "_0");
            break;

        case 13:
            SelectCurrent();
            break;
        default:
            Suggestion(InputValStg, MinCharInt, InputIDStg, URL, ExtFieldName, ExtFieldID, CallBack);
            break;
    }

}

function Suggestion(InputValStg, MinCharInt, InputIDStg, URL, ExtFieldName, ExtFieldID, CallBack) {
    if (InputValStg.length > MinCharInt) {

        var strRegExp = new RegExp(InputValStg, 'g');
        var SuggestionBox = document.createElement('div');
        let list = '';
        if (ExtFieldName != null && ExtFieldName != '') {
            var val = document.getElementById(ExtFieldID).value;
            URL = URL + '?term=' + InputValStg + '&' + ExtFieldName + '=' + val;
        }
        else { URL = URL + '?term=' + InputValStg; }

        fetch(URL).then(
            function (response) {
                return response.json();

            }).then(function (data) {
                //if suggestion is found then show it else show nothing found div
                if (data.length > 0) {
                    for (i = 0; i < data.length; i++) {

                        var ListItem = document.createElement('div');
                        ListItem.setAttribute('class', 'SuggestItem');
                        ListItem.setAttribute('id', InputIDStg + '_' + i);
                        ListItem.setAttribute('tabindex', i);
                        ListItem.innerHTML = data[i].label + "<input type='hidden' value='" + i + "'>";
                        if (CallBack == null || CallBack == undefined) {
                            ListItem.addEventListener('click', SelectSuggestion, false);
                        }
                        else {
                            ListItem.addEventListener('click', CallBack, false);
                        }
                        ListItem.addEventListener('keydown', function (e) { SelectKeySuggestion(e, CallBack); }, false);
                        ListItem.InputID = InputIDStg;
                        ListItem.Id = data[i].id;
                        ListItem.Value = data[i].value;
                        ListItem.Label = data[i].label;
                        ListItem.Text = data[i].text;
                        if (data[i].text == undefined || data[i].text == "" || data[i].text == null) { ListItem.Text = ""; }
                        ListItem.Text1 = data[i].text1;
                        if (data[i].text1 == undefined || data[i].text1 == "" || data[i].text1 == null) { ListItem.Text1 = ""; }
                        ListItem.Text2 = data[i].text2;
                        if (data[i].text2 == undefined || data[i].text2 == "" || data[i].text2 == null) { ListItem.Text2 = ""; }
                        ListItem.Text3 = data[i].text3;
                        if (data[i].text3 == undefined || data[i].text3 == "" || data[i].text3 == null) { ListItem.Text3 = ""; }
                        ListItem.Text4 = data[i].text4;
                        if (data[i].text4 == undefined || data[i].text4 == "" || data[i].text4 == null) { ListItem.Text4 = ""; }

                        if (i === (data.length - 1)) { ListItem.Next = 'none'; }
                        else {
                            var n = i + 1;
                            ListItem.Next = InputIDStg + '_' + n;
                        }
                        if (i === 0) { ListItem.Prev = 'none'; }
                        else {
                            var p = i - 1;
                            ListItem.Prev = InputIDStg + '_' + p;
                        }
                        SuggestionBox.appendChild(ListItem);
                    }
                    AddSuggestionBox(InputIDStg, SuggestionBox);
                    return true;
                }
                else {

                    const div = document.createElement('div');
                    var SuggestionInfo = '<div class="SuggestionInfo SuggestItem SuggestActive"> <div class="SuggestIcon AnRn360"> <i class="UT-GearLine" > </i> </div> <div> No Suggestions Found</div> </div>';
                    div.insertAdjacentHTML('beforeend', SuggestionInfo);
                    AddSuggestionBox(InputIDStg, div);

                }

            })



    }
    else {
        const div = document.createElement('div');
        var SuggestionInfo = '<div class="SuggestionInfo SuggestItem SuggestActive"> <div class="SuggestIcon AnRn360"> <i class="UT-GearLine" > </i> </div> <div> See Suggestion On ' + (MinCharInt + 1) + ' Letter </div> </div>';
        div.insertAdjacentHTML('beforeend', SuggestionInfo);
        AddSuggestionBox(InputIDStg, div);
    }
}

function SelectKeySuggestion(event, CallBack) {
    //To Stop Page Scroll Results from arrow key and Form Submission from Enter Key
    event.preventDefault();
    var index = parseInt(event.target.getAttribute('tabindex'));
    var keycode = event.which;

    switch (keycode) {
        case 38:
            //MoveUp

            SuggestionNavigate(event.target.Prev);
            break;
        case 40:
            //MoveDown();
            index = index + 1;
            SuggestionNavigate(event.target.Next);
            break;
        case 13:
            if (CallBack == null || CallBack == "") {
                SelectSuggestion(event);
            }
            else {
                CallBack(event);
            }
            break;
    }
}

function SuggestionNavigate(NavID) {

    var active = document.getElementsByClassName('SuggestActive');
    for (i = 0; i < active.length; i++) {
        active[i].classList.remove('SuggestActive');
    }

    try {
        var suggestItem = document.getElementById(NavID);
        suggestItem.classList.add('SuggestActive');
        suggestItem.focus();
    }
    catch (e) { }
}


function SuggestionClose(InputId) {
    var oldSBox = document.getElementById(InputId + "SBoxUT");
    if (oldSBox != null) {
        oldSBox.remove();
    }
    document.body.removeAttribute('onclick');
}

function AddSuggestionBox(InputId, Suggestion) {
    //Adding Event on body to close the AutoComplete
    //Close Suggestion if old present
    SuggestionClose(InputId);
    document.body.setAttribute('onclick', "SuggestionClose('" + InputId + "')");

    //Locate the Input Location First
    const element = document.getElementById(InputId);
    const InputXY = element.getBoundingClientRect();
    var leftPosnInputWindow = parseInt(InputXY.left.toFixed()) + 5; //Gives left edge distance from ViewPort
    var scrollTop = (window.pageYOffset !== undefined) ? window.pageYOffset : (document.documentElement || document.body.parentNode || document.body).scrollTop;
    var bottomPosnInputDoc = parseInt(InputXY.bottom.toFixed()) + parseInt(scrollTop) + 2;// Gives Bottom Edge Distance from --Document---
    var topPosnInputWindow = parseInt(InputXY.top.toFixed());
    const viewPortHeight = window.innerHeight;
    //Location Ends
    const body = document.getElementsByTagName("BODY")[0];
    const displaySuggestionDiv = document.createElement('div');
    displaySuggestionDiv.setAttribute('class', 'SuggestionDiv');
    displaySuggestionDiv.setAttribute('id', InputId + 'SBoxUT');
    const SuggestBox = document.createElement('div');
    SuggestBox.setAttribute('class', 'SuggestionBox');
    SuggestBox.appendChild(Suggestion);
    displaySuggestionDiv.appendChild(SuggestBox);
    body.appendChild(displaySuggestionDiv);
    //Get the of Div Created TO Show
    var SuggestBoxHeight = document.getElementById(InputId + 'SBoxUT').clientHeight;
    //IF WindowHEIGHT-InputTop > DivCreatedheight then open at bottom of input.
    if ((viewPortHeight - parseInt(InputXY.bottom.toFixed())) >= SuggestBoxHeight) {
        document.getElementById(InputId + 'SBoxUT').style.top = bottomPosnInputDoc + "px";
        document.getElementById(InputId + 'SBoxUT').style.left = leftPosnInputWindow + "px";
    }
    //Top Opening Position Of DivCreated  = Scroll + WindowTopOfInput -HeightofDiv
    else {
        document.getElementById(InputId + 'SBoxUT').style.top = (parseInt(scrollTop) + topPosnInputWindow - SuggestBoxHeight - 2) + "px";
        document.getElementById(InputId + 'SBoxUT').style.left = leftPosnInputWindow + "px";
    }
}
///////////////////////////////////AutoCOmplete Ends///////////////////////////////////////////////////////


/////////////////////////////////TableSearch/////////////////////////////////////////////////////////
function StartTableSearch(tableID, SkipRow, SkipCol) {
    var Table = document.getElementById(tableID);
    var TableColNumber = Table.rows[0].cells.length;
    for (i = 0; i < TableColNumber; i++) {
        var CurrentCell = Table.rows[0].cells[i];
        CurrentCell.setAttribute('tabindex', i);
        CurrentCell.addEventListener('keyup', function (e) { SearchTable(e, tableID, SkipRow, SkipCol) });
        CurrentCell.TableID = tableID;
    }
}
function SearchTable(event, TableID, SkipRow, SkipCol) {
    //const cell = event.target.closest('td');will be used for single search
    var Table = document.getElementById(TableID);
    var TableRowsCount = Table.rows.length;
    var TableRowCellsCount = Table.rows[0].cells.length;
    //var TBodyRowsCount = Table.tBodies[0].rows.length;


    for (i = 0; i < TableRowsCount; i++) {
        var ShowRow = true;

        if (!SkipRow.includes(i)) {

            for (j = 0; j < TableRowCellsCount; j++) {
                if (!SkipCol.includes(j)) {
                    var cell = document.getElementById(TableID).rows[0].cells[j];
                    var SearchValue = cell.children[0].value.toLowerCase();
                    var tdIndexSearch = cell.cellIndex;
                    if (SearchValue !== "") {

                        var value = document.getElementById(TableID).rows[i].cells[tdIndexSearch].innerHTML.toLowerCase();
                        if (value.includes(SearchValue)) {
                            ShowRow = true;
                        }
                        else {
                            ShowRow = false;
                            break;
                        }
                    }
                }
            }

            if (ShowRow) {
                try {
                    document.getElementById(TableID).rows[i].removeAttribute("hidden");
                }
                catch (er) { }
            }
            else {
                document.getElementById(TableID).rows[i].setAttribute('hidden', 'hidden');

            }

            //After Checking The Row Will Decide To Show or Hide

        }
    }

}
function SearchClear(TableID) {
    //const cell = event.target.closest('td');will be used for single search
    var Table = document.getElementById(TableID);
    var TableRowsCount = Table.rows.length;
    var TableRowCellsCount = Table.rows[0].cells.length;
    //var TBodyRowsCount = Table.tBodies[0].rows.length;
    for (i = 0; i < TableRowsCount; i++) {

        for (j = 0; j < TableRowCellsCount; j++) {
            document.getElementById(TableID).rows[0].cells[j].children[0].value = "";
        }
        try {
            document.getElementById(TableID).rows[i].removeAttribute("hidden");
        }
        catch (er) { }
    }
}
/////////////////////////Table Search Ends Here//////////////////////////////////////////

//////////////////Filter Search Script Starts////////////////////////////////////////////////////
function AddSearchInput() {
    //Get the Intial Value to Create Form Element like List Count
    var DropDown = document.getElementById('SearchInputOpt');
    var DnText = DropDown.options[DropDown.selectedIndex].text;
    var SearchCounter = parseInt(document.getElementById('SearchCounter').value);//This Gives the Count Number
    var SearchInputDiv = document.getElementById('SearchInputDiv');//Tells In Which Div Id to Add Field
    var ID = 'ID' + SearchCounter;//Provides the ID For Generated Field
    var Name = DnText + SearchCounter;//Provides the Field Name
    var DnValue = DropDown.options[DropDown.selectedIndex].value;//Provides the User Selected Value


    switch (DnValue) {

        case "string": SearchInputDiv.appendChild(CreateSearchString(ID, 'IndexSearchList[' + SearchCounter + '].TextSearch', DnText, SearchCounter));
            break;
        case "bool": SearchInputDiv.appendChild(CreateSearchCheck(ID, 'IndexSearchList[' + SearchCounter + '].BoolSearch', DnText, SearchCounter));
            break;
        case "DateTime": SearchInputDiv.appendChild(CreateSearchDate(ID, 'IndexSearchList[' + SearchCounter + '].DateSearch', DnText, SearchCounter));
            break;
        case "int": SearchInputDiv.appendChild(CreateSearchNumber(ID, 'IndexSearchList[' + SearchCounter + '].IntSearch', DnText, SearchCounter));
            break;
    }
    document.getElementById('SearchCounter').value = SearchCounter + 1;

}

function CreateMandatoryField(Count, Name, Type) {
    var div = document.createElement('div');
    var inputName = document.createElement('input');
    inputName.setAttribute('type', 'hidden');
    inputName.setAttribute('name', 'IndexSearchList[' + Count + '].Name');
    inputName.setAttribute('value', Name);


    var inputType = document.createElement('input');
    inputType.setAttribute('name', 'IndexSearchList[' + Count + '].Type');
    inputType.setAttribute('value', Type);

    inputType.setAttribute('type', 'hidden');
    div.appendChild(inputName);
    div.appendChild(inputType);
    return div;
}

function CreateSearchCheck(ID, Name, InputName, SearchCounter) {
    var SearchContainer = document.createElement('div');
    SearchContainer.setAttribute('id', ID);
    SearchContainer.setAttribute('class', 'SearchItem MnTp15 MnBm15');
    SearchContainer.appendChild(CreateMandatoryField(SearchCounter, InputName, 'bool'));
    var ShCrCd1 = document.createElement('div');
    ShCrCd1.setAttribute('class', 'TtBxFlStDiv FxRw Pg10');
    SearchContainer.appendChild(ShCrCd1);
    var ShCrCd1Cd1 = document.createElement('div');
    ShCrCd1Cd1.setAttribute('class', 'FxGw1');
    ShCrCd1.appendChild(ShCrCd1Cd1);
    var ShCrCd1Cd1Cd1 = document.createElement('input');
    ShCrCd1Cd1Cd1.setAttribute('class', 'SeIt1o15');
    ShCrCd1Cd1Cd1.setAttribute('type', 'checkbox');
    ShCrCd1Cd1Cd1.setAttribute('value', 'true');
    ShCrCd1Cd1Cd1.setAttribute('name', Name);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd1);
    ShCrCd1Cd1.insertAdjacentHTML("beforeend", InputName);
    var ShCrCd1Cd2 = document.createElement('div');
    ShCrCd1Cd2.setAttribute('class', 'Cr-uPr MnRt10 DyFx FxAnImCr FxJyCtCr');
    ShCrCd1Cd2.setAttribute('onclick', "RemoveSearchItem('" + ID + "')");
    var ShCrCd1Cd2CD1 = document.createElement('i');
    ShCrCd1Cd2CD1.setAttribute('class', 'UT-DeletePageSolid');
    ShCrCd1Cd2.appendChild(ShCrCd1Cd2CD1);
    ShCrCd1.appendChild(ShCrCd1Cd2);
    return SearchContainer;
}

function CreateSearchNumber(ID, Name, InputName, SearchCounter) {
    var DateContainer = document.createElement('div');
    DateContainer.setAttribute('id', ID);
    DateContainer.setAttribute('class', 'FxRw MnTp15 MnBm15');
    DateContainer.appendChild(CreateMandatoryField(SearchCounter, InputName, 'int'));
    DateContainer.appendChild(CreateSearchNumberComponent(ID, "From" + Name, "Start " + InputName, 'PgRt5 Wh50p'));
    DateContainer.appendChild(CreateSearchNumberComponent(ID, "To" + Name, "End " + InputName, 'PgLt5 Wh50p'));
    return DateContainer;
}

function CreateSearchNumberComponent(ID, Name, InputName, Class) {
    var SearchContainer = document.createElement('div');
    SearchContainer.setAttribute('class', Class);
    var ShCrCd1 = document.createElement('div');
    ShCrCd1.setAttribute('class', 'TtBxFlStDiv');
    SearchContainer.appendChild(ShCrCd1);
    var ShCrCd1Cd1 = document.createElement('div');
    ShCrCd1Cd1.setAttribute('class', 'TtBxFlSt');
    ShCrCd1.appendChild(ShCrCd1Cd1);
    var ShCrCd1Cd1Cd1 = document.createElement('div');
    ShCrCd1Cd1Cd1.setAttribute('class', 'TtBxFlStLl15');
    ShCrCd1Cd1Cd1.innerText = InputName;
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd1);
    var ShCrCd1Cd1Cd2 = document.createElement('input');
    ShCrCd1Cd1Cd2.setAttribute('class', 'TtBxTt15');
    ShCrCd1Cd1Cd2.setAttribute('type', 'text');
    ShCrCd1Cd1Cd2.setAttribute('name', Name);
    ShCrCd1Cd1Cd2.setAttribute('placeholder', InputName);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd2);
    var ShCrCd1Cd1Cd3 = document.createElement('div');
    ShCrCd1Cd1Cd3.setAttribute('class', 'Cr-uPr MnRt10');
    ShCrCd1Cd1Cd3.setAttribute('onclick', "RemoveSearchItem('" + ID + "')");
    var ShCrCd1Cd1Cd3CD1 = document.createElement('i');
    ShCrCd1Cd1Cd3CD1.setAttribute('class', 'UT-DeletePageSolid');
    ShCrCd1Cd1Cd3.appendChild(ShCrCd1Cd1Cd3CD1);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd3);
    return SearchContainer;
}

function CreateSearchDate(ID, Name, InputName, SearchCounter) {
    var DateContainer = document.createElement('div');
    DateContainer.setAttribute('id', ID);
    DateContainer.setAttribute('class', 'FxRw MnTp15 MnBm15');
    DateContainer.appendChild(CreateMandatoryField(SearchCounter, InputName, 'DateTime'));
    var FromDate = CreateSearchDateComponent(ID, Name + "From", InputName + " From", "from", 'PgRt5 Wh50p');
    DateContainer.appendChild(FromDate);
    DateContainer.appendChild(CreateSearchDateComponent(ID, Name + "To", InputName + " To", "to", 'PgLt5 Wh50p'));
    return DateContainer;
}

function CreateSearchDateComponent(ID, Name, InputName, DateFor, Class) {
    var SearchContainer = document.createElement('div');
    SearchContainer.setAttribute('class', Class);
    var ShCrCd1 = document.createElement('div');
    ShCrCd1.setAttribute('class', 'TtBxFlStDiv');
    SearchContainer.appendChild(ShCrCd1);
    var ShCrCd1Cd1 = document.createElement('div');
    ShCrCd1Cd1.setAttribute('class', 'TtBxFlSt');
    ShCrCd1.appendChild(ShCrCd1Cd1);
    var ShCrCd1Cd1Cd1 = document.createElement('div');
    ShCrCd1Cd1Cd1.setAttribute('class', 'TtBxFlStLl15');
    ShCrCd1Cd1Cd1.innerText = InputName;
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd1);
    var ShCrCd1Cd1Cd2 = document.createElement('input');
    ShCrCd1Cd1Cd2.setAttribute('class', 'TtBxTt15');
    ShCrCd1Cd1Cd2.setAttribute('id', ID + DateFor);
    ShCrCd1Cd1Cd2.setAttribute('utdate', 'D-MM-YYYY');
    ShCrCd1Cd1Cd2.setAttribute('onclick', "ShowCalender('" + ID + DateFor + "')");
    ShCrCd1Cd1Cd2.setAttribute('type', 'text');
    ShCrCd1Cd1Cd2.setAttribute('name', Name);
    ShCrCd1Cd1Cd2.setAttribute('placeholder', InputName);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd2);
    var ShCrCd1Cd1Cd3 = document.createElement('div');
    ShCrCd1Cd1Cd3.setAttribute('class', 'Cr-uPr MnRt10');
    ShCrCd1Cd1Cd3.setAttribute('onclick', "RemoveSearchItem('" + ID + "')");
    var ShCrCd1Cd1Cd3CD1 = document.createElement('i');
    ShCrCd1Cd1Cd3CD1.setAttribute('class', 'UT-DeletePageSolid');
    ShCrCd1Cd1Cd3.appendChild(ShCrCd1Cd1Cd3CD1);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd3);
    return SearchContainer;
}

function CreateSearchString(ID, Name, InputName, SearchCounter) {
    var SearchContainer = document.createElement('div');
    SearchContainer.setAttribute('id', ID);
    SearchContainer.setAttribute('class', 'SearchItem MnTp15 MnBm15');

    SearchContainer.appendChild(CreateMandatoryField(SearchCounter, InputName, 'string'));
    var ShCrCd1 = document.createElement('div');
    ShCrCd1.setAttribute('class', 'TtBxFlStDiv');
    SearchContainer.appendChild(ShCrCd1);
    var ShCrCd1Cd1 = document.createElement('div');
    ShCrCd1Cd1.setAttribute('class', 'TtBxFlSt');
    ShCrCd1.appendChild(ShCrCd1Cd1);
    var ShCrCd1Cd1Cd1 = document.createElement('div');
    ShCrCd1Cd1Cd1.setAttribute('class', 'TtBxFlStLl15');
    ShCrCd1Cd1Cd1.innerText = InputName;
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd1);
    var ShCrCd1Cd1Cd2 = document.createElement('input');
    ShCrCd1Cd1Cd2.setAttribute('class', 'TtBxTt15');
    ShCrCd1Cd1Cd2.setAttribute('type', 'text');
    ShCrCd1Cd1Cd2.setAttribute('name', Name);
    ShCrCd1Cd1Cd2.setAttribute('placeholder', InputName);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd2);
    var ShCrCd1Cd1Cd3 = document.createElement('div');
    ShCrCd1Cd1Cd3.setAttribute('class', 'Cr-uPr MnRt10');
    ShCrCd1Cd1Cd3.setAttribute('onclick', "RemoveSearchItem('" + ID + "')");
    var ShCrCd1Cd1Cd3CD1 = document.createElement('i');
    ShCrCd1Cd1Cd3CD1.setAttribute('class', 'UT-DeletePageSolid');
    ShCrCd1Cd1Cd3.appendChild(ShCrCd1Cd1Cd3CD1);
    ShCrCd1Cd1.appendChild(ShCrCd1Cd1Cd3);
    return SearchContainer;
}

function RemoveSearchItem(ID) {
    document.getElementById(ID).remove();
}
///////////////////////////////Fiter Search Script Ends//////////////////////////////////////////////

function TypeWriter(IDStg, SpeedInt, CursorStg) {
    var contentdiv = document.getElementById(IDStg);
    var content = contentdiv.innerHTML;
    contentdiv.innerHTML = " ";
    contentdiv.style.display = "block";
    var i = 0;
    var Ct = content.length;
    var typer = setInterval(function () {
        if (i > 0) {
            var text = contentdiv.innerHTML.replace(CursorStg, "");
            contentdiv.innerHTML = text;
            contentdiv.insertAdjacentText('beforeend', content.charAt(i) + CursorStg);

        }
        if (i == 0) {
            contentdiv.insertAdjacentText('beforeend', content.charAt(i));
        }
        if (Ct < i) {
            var text = contentdiv.innerHTML.replace(CursorStg, "");
            contentdiv.innerHTML = text;
            clearInterval(typer);
        }
        i++;

    }, SpeedInt);
}

function delay(time) {
    return new Promise(resolve => setTimeout(resolve, time));
}
function ShowNavSideMe() {
    document.getElementById('NavSideID').style.zIndex = "100000000";
    document.getElementById('NavSideID').style.left = "0%";
    document.getElementById('NavSideID').style.width = "90%";
    document.getElementById('SideNavSectionID').style.minHeight = "90%";
    document.getElementById('NavSideID').style.minHeight = "96%";
    document.getElementById('NavSideID').style.backgroundColor = "white";

}
function HideNavSideMe() {
    document.getElementById('NavSideID').style.left = "-50%";
    document.getElementById('NavSideID').style.width = "17.5%";
    document.getElementById('SideNavSectionID').style.minHeight = "auto";
    document.getElementById('NavSideID').style.minHeight = "auto";
    document.getElementById('NavSideID').style.backgroundColor = "transparent";

}
function ShowNavSide() {
    /* alert(1);*/
    document.getElementById('NavSideShowButtonID').style.display = 'none';
    document.getElementById('NavSideID').style.width = '17.5%';
    document.getElementById('SideNavSectionID').style.opacity = '1',

        delay(200).then(() =>
            document.getElementById('SideNavSectionID').style.display = 'block'
        );
    document.getElementById('NavSideContentID').style.width = '82.5%';
    document.getElementById('NavSideContentID').style.marginLeft = '0';

}

window.onload = function () {
    // SideNavigationStarts
    var NavLink = document.getElementsByClassName("SideNavLink");
    var NavSubCont = document.getElementsByClassName("SideSubNavContainer");
    var i;
    for (var i = 0; i < NavLink.length; i++) {

        NavLink[i].addEventListener("click", function () {

            if (false == this.classList.contains("SideNavActive")) {
                for (var i = 0; i < NavLink.length; i++) {
                    try {
                        NavLink[i].classList.remove("SideNavActive");
                    }
                    catch { }
                    try {
                        NavSubCont[i].style.maxHeight = null;
                        NavSubCont[i].classList.remove("SideSubNavActive");

                    }
                    catch { }
                }
                try {
                    var NavSubContV = this.parentElement.nextElementSibling;
                    if (NavSubContV.style.maxHeight) {
                        NavSubContV.style.maxHeight = null;
                    } else {
                        NavSubContV.style.maxHeight = NavSubContV.scrollHeight + "px";
                    }
                }
                catch { }
                this.classList.toggle("SideNavActive");
            }
            else {
                for (var i = 0; i < NavLink.length; i++) {
                    try {
                        NavLink[i].classList.remove("SideNavActive");
                    }
                    catch { }
                    try {
                        NavSubCont[i].style.maxHeight = null;
                        NavSubCont[i].classList.remove("SideSubNavActive");
                    }
                    catch { }
                }
            }
        });
    }


    //SideNavgation Ends



    document.getElementById('SideNavHideBnID').addEventListener("click", function () {


        delay(250).then(() =>
            document.getElementById('SideNavSectionID').style.display = 'none',
            document.getElementById('SideNavSectionID').style.opacity = '0'
        );
        document.getElementById('NavSideID').style.width = "0";


        document.getElementById('NavSideShowButtonID').style.display = 'block';
        document.getElementById('NavSideContentID').style.width = '98%';
        document.getElementById('NavSideContentID').style.marginLeft = '2%';

    });

}
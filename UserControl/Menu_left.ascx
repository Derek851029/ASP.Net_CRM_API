<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Menu_left.ascx.cs" Inherits="Menu_left" %>
<body id="body-pd">
<header class="header" id="header">
        <div class="header_toggle"> 
            <i class='bx bx-menu' id="header-toggle"></i> 
            <ul class="navbar-nav float-end">
                        <li class="nav-item dropdown">
                            <a class="nav-link dropdown-toggle text-muted waves-effect waves-dark pro-pic" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown" aria-expanded="false">
                                <img src="../assets/images/users/profile.png" alt="user" class="rounded-circle" width="31">
                            </a>
                            <ul class="dropdown-menu dropdown-menu-end user-dd animated" aria-labelledby="navbarDropdown">
                                <a class="dropdown-item" href="javascript:void(0)"><i class="ti-user m-r-5 m-l-5"></i>
                                    My Profile</a>
                                <a class="dropdown-item" href="javascript:void(0)"><i class="ti-wallet m-r-5 m-l-5"></i>
                                    My Balance</a>
                                <a class="dropdown-item" href="javascript:void(0)"><i class="ti-email m-r-5 m-l-5"></i>
                                    Inbox</a>
                            </ul>
                        </li>
                    </ul>
        </div>
</header>
<div class="l-navbar" id="nav-bar">
    <nav class="nav_list">
        <div> 
            <a href="Default.aspx" class="nav_logo"> 
                <i class='bx bx-layer nav_logo-icon'></i> 
                <span class="nav_logo-name">智慧藥妝</span> 
            </a>
            <div class="nav_list"> 
                <%--<a class="nav_link" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown">
                    <i class='bx bx-grid-alt nav_icon'></i> 
                    <span class="nav_name">Dashboard</span>
                </a>
                <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                    <li><a class="dropdown-item" href="#">Action</a></li>
                    <li><a class="dropdown-item" href="#">Another action</a></li>
                    <li><hr class="dropdown-divider"></li>
                    <li><a class="dropdown-item" href="#">Something else here</a></li>
                </ul>--%>
                <a href="Dashboard.aspx" id="dashboard" class="nav_link"> 
                    <i class='bi-card-list' style="font-size: 1.5rem;"></i> 
                    <span class="nav_name">儀表板</span> 
                </a> 

                <a class="nav_link" href="Default.aspx" id="member" role="button" data-bs-toggle="dropdown">
                    <i class='bi-person-circle' style="font-size: 1.5rem;"></i> 
                    <span class="nav_name">基本資料維護</span>
                </a>
                <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                    <li><a class="dropdown-item" href="Member.aspx">會員管理</a></li>
                    <li><a class="dropdown-item" href="0060010004.aspx">員工管理</a></li>
                    <li><a class="dropdown-item" href="#">消費紀錄查詢</a></li>
                </ul>
                <div id="member_div" style="height:60px;display:none"></div>

                <a class="nav_link" href="#" id="Commodity" role="button" data-bs-toggle="dropdown">
                    <i class='bx bx-grid-alt nav_icon'></i> 
                    <span class="nav_name">商品管理</span>
                </a>
                <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                    <li><a class="dropdown-item" href="CommodityList.aspx">商品資訊</a></li>
                    <li><a class="dropdown-item" href="#">商品查詢</a></li>
                </ul>
                <div id="Commodity_div" style="height:85px;display:none"></div>

                <a href="0060010001.aspx" id="System" class="nav_link"> 
                    <i class='bi-gear' style="font-size: 1.5rem;"></i> 
                    <span class="nav_name">系統參數維護</span> 
                </a> 

                <%--<a href="Report.aspx" class="nav_link"> 
                    <i class='bx bx-bookmark nav_icon'></i> 
                    <span class="nav_name">Report</span> 
                </a>--%>

                <%--</a> 
                <a href="#" class="nav_link"> 
                    <i class='bx bx-folder nav_icon'></i> 
                    <span class="nav_name">Files</span> 

                </a> 
                <a href="#" class="nav_link"> 
                    <i class='bx bx-bar-chart-alt-2 nav_icon'></i> 
                    <span class="nav_name">Stats</span> 

                </a> --%>

            </div>
        </div> 
        <a href="#" class="nav_link" onserverclick="Logout" runat="server"> 
            <i class='bx bx-log-out nav_icon'></i> 
            <span class="nav_name">SignOut</span> 

        </a>
    </nav>
</div>
    </body>
<script>
    var RoleID = <%=Session["RoleID"]%>
    $(function () {
        console.log(RoleID)
        $('#member').click(function () {
            $('#member_div').slideToggle('slow')
            if ($('#Commodity_div').is(':visible')) {
                $('#Commodity_div').slideToggle('slow')
            }
            
        })
        $('#Commodity').click(function () {
            $('#Commodity_div').slideToggle('slow')
            if ($('#member_div').is(':visible')) {
                $('#member_div').slideToggle('slow')
            }
        })
        $(document).click(function (event) {
            var member_div = $('#member')
            var Commodity_div = $('#Commodity_div')
            if ($('#member_div').is(':visible')) {
                if (!member_div.is(event.target) && member_div.has(event.target).length === 0) {
                    $('#member_div').slideToggle('slow')
                }
            }
            if ($('#Commodity_div').is(':visible')) {
                if (!Commodity_div.is(event.target) && Commodity_div.has(event.target).length === 0) {
                    $('#Commodity_div').slideToggle('slow')
                }
            }
        })
    })
</script>
<link href="https://cdn.jsdelivr.net/npm/boxicons@latest/css/boxicons.min.css" rel="stylesheet"/>
<style>
    @import url("https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700&display=swap");

:root {
    --header-height: 3rem;
    --nav-width: 68px;
/*    --first-color: #4723D9;*/
    --first-color-light: #AFA5D9;
/*    --white-color: #F7F6FB;*/
    --body-font: 'Nunito', sans-serif;
    --normal-font-size: 1rem;
    --z-fixed: 100
}

*,
::before,
::after {
    box-sizing: border-box
}

body {
    position: relative;
    margin: var(--header-height) 0 0 0;
    padding: 0 1rem;
    font-family: var(--body-font);
    font-size: var(--normal-font-size);
    transition: .5s
}

a {
    text-decoration: none
}

.header {
    width: 100%;
    height: var(--header-height);
    position: fixed;
    top: 0;
    left: 0;
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 0 1rem;
    background-color: var(--white-color);
    z-index: var(--z-fixed);
    transition: .5s
}

.header_toggle {
    color: var(--first-color);
    font-size: 1.5rem;
    cursor: pointer;
    width: 100%;
}

.header_img {
    width: 35px;
    height: 35px;
    display: flex;
    justify-content: center;
    border-radius: 50%;
    overflow: hidden
}

.header_img img {
    width: 40px
}

.l-navbar {
    position: fixed;
    top: 0;
    left: -30%;
    width: var(--nav-width);
    height: 100vh;
    background-color: var(--first-color);
    padding: .5rem 1rem 0 0;
    transition: .5s;
    z-index: var(--z-fixed)
}

.nav_list {
    height: 100%;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
    overflow: hidden
}

.nav_logo,
.nav_link {
    display: grid;
    grid-template-columns: max-content max-content;
    align-items: center;
    column-gap: 1rem;
    padding: .5rem 0 .5rem 1.5rem
}

.nav_logo {
    margin-bottom: 2rem
}

.nav_logo-icon {
    font-size: 1.25rem;
    color: var(--white-color)
}

.nav_logo-name {
    color: var(--white-color);
    font-weight: 700
}

.nav_link {
    position: relative;
    color: var(--first-color-light);
    margin-bottom: 1.5rem;
    transition: .3s;
    border-radius:10px;
    padding-left:30%
}

.nav_link:hover {
    color: var(--white-color)
}

.nav_icon {
    font-size: 1.25rem
}

.show {
    left: 0
}

.body-pd {
    padding-left: calc(var(--nav-width) + 1rem)
}

.active {
    color: var(--white-color)
}

.active::before {
    content: '';
    position: absolute;
    left: 0;
    width: 2px;
    height: 32px;
    background-color: var(--white-color)
}

.height-100 {
    height: 100vh
}

@media screen and (min-width: 768px) {
    body {
        margin: calc(var(--header-height) + 1rem) 0 0 0;
        padding-left: calc(var(--nav-width) + 2rem)
    }

    .header {
        height: calc(var(--header-height) + 1rem);
        padding: 0 2rem 0 calc(var(--nav-width) + 2rem)
    }

    .header_img {
        width: 40px;
        height: 40px
    }

    .header_img img {
        width: 45px
    }

    .l-navbar {
        left: 0;
        padding: 1rem 1rem 0 0
    }

    .show:not(#dialog1):not(.modal-backdrop):not(#dialog2):not(#dialog3):not(#navbarDropdown):not(#myModal):not(#Div1):not(#newModal) {
        width: calc(var(--nav-width) + 190px)
    }

    .body-pd {
        padding-left: calc(var(--nav-width) + 188px)
    }

    ul {
        list-style-type: none;
     }
}
</style>
<script>
    $(function () {
        const showNavbar = (toggleId, navId, bodyId, headerId) => {
            const toggle = document.getElementById(toggleId),
                nav = document.getElementById(navId),
                bodypd = document.getElementById(bodyId),
                headerpd = document.getElementById(headerId)
            // Validate that all variables exist
            if (toggle && nav && bodypd && headerpd) {
                toggle.addEventListener('click', () => {
                    // show navbar
                    nav.classList.toggle('show')
                    // change icon
                    toggle.classList.toggle('bx-x')
                    // add padding to body
                    bodypd.classList.toggle('body-pd')
                    // add padding to header
                    headerpd.classList.toggle('body-pd')
                })
            }
        }

        showNavbar('header-toggle', 'nav-bar', 'body-pd', 'header')

        /*===== LINK ACTIVE =====*/
        const linkColor = document.querySelectorAll('.nav_link')

        function colorLink() {
            if (linkColor) {
                linkColor.forEach(l => l.classList.remove('active'))
                this.classList.add('active')
            }
        }
        linkColor.forEach(l => l.addEventListener('click', colorLink))

        // Your code to run since DOM is loaded and ready
    })
</script>
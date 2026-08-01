<%@ Page Language="C#"
    AutoEventWireup="true"
    CodeBehind="PB_page.aspx.cs"
    Inherits="PFF.PB_page"
    ResponseEncoding="utf-8"
    Culture="en-US"
    UICulture="en" %>
<!DOCTYPE html>
<html lang="en">
<head runat="server">
<meta charset="utf-8" />
<meta name="viewport" content="width=device-width, initial-scale=1" />
<title>Production Board</title>
<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.2/css/all.min.css" />
<style>
:root{--blue:#1f709d;--light:#edf7fb;--ink:#263743;--line:#253743;--red:#aa4039}
*{box-sizing:border-box}html,body{min-height:100%;margin:0}body{font-family:Arial,Helvetica,sans-serif;color:var(--ink);background:#fff}button,input,textarea{font:inherit}
.side{position:fixed;inset:0 auto 0 0;z-index:20;width:72px;overflow:hidden;border-right:1px solid #dce7ec;background:#fff;box-shadow:7px 0 24px rgba(30,75,99,.07);transition:.2s}.side.open{width:230px}.toggle{width:100%;height:72px;border:0;cursor:pointer;color:var(--blue);background:#fff;font-size:27px}.nav{display:flex;flex-direction:column;gap:7px;padding:7px 10px}.nav a{display:flex;align-items:center;height:47px;padding:0 14px;border-radius:11px;color:#5c7481;text-decoration:none;white-space:nowrap}.nav a:hover,.nav a.active{color:var(--blue);background:var(--light)}.nav i{width:32px;text-align:center}.nav span{margin-left:10px;opacity:0;font-size:14px;font-weight:700;transition:.15s}.side.open .nav span{opacity:1}
.page{min-height:100vh;margin-left:72px;padding:12px 14px 16px;transition:.2s}.page.open{margin-left:230px}.wrap{width:100%;max-width:none;margin:0}.toolbar{display:flex;justify-content:space-between;align-items:flex-start;gap:14px;margin-bottom:9px}.toolbar h1{margin:0;color:#1e506b;font-size:clamp(22px,1.7vw,28px)}.toolbar p{margin:6px 0 0;color:#697b86;font-size:13px}.actions{display:flex;gap:8px;flex-wrap:wrap}.actions a{display:inline-flex;align-items:center;gap:7px;min-height:40px;padding:0 13px;border:1px solid #d2e0e7;border-radius:9px;color:var(--blue);background:#fff;text-decoration:none;font-size:13px;font-weight:700}
.board{overflow:hidden;border:2px solid var(--line);background:#fff;box-shadow:0 10px 30px rgba(30,70,92,.08)}.board-title{padding:8px 16px;border-bottom:2px solid var(--line);text-align:center}.board-title h2{margin:0;color:#17262e;font-size:clamp(26px,2.4vw,38px);font-weight:800}.info{display:grid;grid-template-columns:minmax(125px,.9fr) minmax(125px,.9fr) minmax(125px,.9fr) minmax(160px,1.15fr) minmax(190px,1.35fr);border-bottom:2px solid var(--line)}.info-box{display:grid;grid-template-columns:auto 1fr;min-height:52px;border-right:2px solid var(--line)}.info-box:last-child{border-right:0}.info-key{display:flex;align-items:center;padding:0 9px;border-right:1px solid var(--line);background:#f7f9fa;font-size:13px;font-weight:800;text-transform:uppercase}.info-value{display:flex;align-items:center;min-width:0;padding:0 10px;overflow:hidden;color:var(--blue);font-size:clamp(14px,1.15vw,19px);font-weight:800;text-overflow:ellipsis;white-space:nowrap}
.scroll{overflow-x:auto}table{width:100%;min-width:930px;border-collapse:collapse;table-layout:fixed}th,td{border-right:2px solid var(--line);border-bottom:2px solid var(--line)}tr>*:last-child{border-right:0}tbody tr:last-child td{border-bottom:0}thead th{height:60px;background:#fff}.clock{width:88px;font-size:24px}.metric{position:relative;width:135px;overflow:hidden}.metric:after,.diag:after{content:"";position:absolute;inset:0;pointer-events:none;background:linear-gradient(to top right,transparent calc(50% - 1px),var(--line) 50%,transparent calc(50% + 1px))}.metric .top,.diag .top{position:absolute;top:8px;right:9px;z-index:1}.metric .bottom,.diag .bottom{position:absolute;bottom:8px;left:9px;z-index:1}.comments{padding:0 18px;font-size:20px;text-align:center}.hour-row{cursor:pointer;outline:none}.hour-row:hover,.hour-row:focus{background:#f7fbfd}.hour-row td{height:clamp(50px,calc((100vh - 270px)/8),74px)}.hour{width:88px;padding:5px;text-align:center}.hour small{display:block;margin-bottom:3px;color:#667984;font-size:10px;font-weight:800}.time{display:inline-flex;align-items:center;justify-content:center;min-width:62px;height:30px;padding:0 6px;border:1px solid #9fc5d8;border-radius:999px;color:var(--blue);background:var(--light);font-size:16px;font-weight:800}.diag{position:relative;width:135px;overflow:hidden;font-size:18px}.diag .top,.diag .bottom{font-size:18px}.actual .top,.actual .bottom{color:#176f99}.scrap .top,.scrap .bottom{color:var(--red)}.comment{position:relative;min-width:340px;padding:10px 88px 10px 14px;text-align:left}.comment span{display:block;color:#354a56;line-height:1.4;word-break:break-word}.comment em{position:absolute;top:50%;right:12px;display:flex;align-items:center;gap:5px;transform:translateY(-50%);opacity:0;color:var(--blue);font-size:11px;font-style:normal;font-weight:800}.hour-row:hover .comment em,.hour-row:focus .comment em{opacity:1}.status{display:block;margin-top:11px;color:#b42318;font-size:13px;font-weight:700}
dialog{width:calc(100% - 28px);max-width:560px;padding:0;border:0;border-radius:16px;background:#fff;box-shadow:0 28px 90px rgba(15,47,65,.32)}dialog::backdrop{background:rgba(17,39,52,.5);backdrop-filter:blur(3px)}.modal-head{display:flex;justify-content:space-between;gap:14px;padding:20px 22px 17px;border-bottom:1px solid #dce5e9;background:#f8fbfc}.modal-head h3{margin:0;color:#174e6b;font-size:21px}.modal-head p{margin:5px 0 0;color:#6b7b85;font-size:13px}.x{width:35px;height:35px;border:1px solid #d4e0e6;border-radius:9px;cursor:pointer;color:#536b78;background:#fff}.hour-nav{display:grid;grid-template-columns:38px minmax(0,1fr) 38px;align-items:center;gap:9px;padding:12px 22px;border-bottom:1px solid #dce5e9;background:#fff}
.hour-nav-arrow{display:inline-flex;align-items:center;justify-content:center;width:38px;height:36px;border:1px solid #cad9e1;border-radius:9px;cursor:pointer;color:var(--blue);background:#fff}
.hour-nav-arrow:hover:not(:disabled){border-color:#79aac3;background:var(--light)}
.hour-nav-arrow:disabled{cursor:not-allowed;opacity:.35}
.hour-tabs{display:grid;grid-template-columns:repeat(8,minmax(34px,1fr));gap:6px}
.hour-tab{position:relative;min-width:0;height:34px;padding:0 5px;border:1px solid #cad9e1;border-radius:8px;cursor:pointer;color:#536b78;background:#fff;font-size:12px;font-weight:800}
.hour-tab:hover{border-color:#79aac3;color:var(--blue);background:#f7fbfd}
.hour-tab.active{border-color:var(--blue);color:#fff;background:var(--blue)}
.hour-tab.dirty:after{content:"";position:absolute;top:4px;right:4px;width:6px;height:6px;border-radius:50%;background:#ef8b2c}
.hour-tab.active.dirty:after{background:#ffd8a8}
.modal-body{padding:21px 22px 23px}.grid{display:grid;grid-template-columns:1fr 1fr;gap:14px}.field.full{grid-column:1/-1}.field label{display:block;margin-bottom:7px;color:#354c59;font-size:13px;font-weight:800}.input{width:100%;min-height:45px;padding:10px 12px;border:1px solid #cbd9e0;border-radius:9px;outline:none}.input:focus{border-color:#65a6c8;box-shadow:0 0 0 4px rgba(101,166,200,.14)}textarea.input{min-height:105px;resize:vertical}.error{display:none;margin-top:11px;padding:10px 12px;border:1px solid #f1c7c3;border-radius:8px;color:#b42318;background:#fff5f4;font-size:13px}.modal-actions{display:flex;justify-content:flex-end;gap:9px;margin-top:19px}.btn{min-height:42px;padding:0 17px;border-radius:9px;cursor:pointer;font-size:13px;font-weight:800}.cancel{border:1px solid #cbd9e0;color:#405763;background:#fff}.save-stay{border:1px solid var(--blue);color:var(--blue);background:#fff}.save-stay:hover{background:#f2f9fc}.save{border:1px solid var(--blue);color:#fff;background:var(--blue)}.success{display:none;margin-top:11px;padding:10px 12px;border:1px solid #b7dfc4;border-radius:8px;color:#176b35;background:#f0fbf4;font-size:13px;font-weight:700}
@media(min-width:1600px){.page{padding:14px 18px 18px}.toolbar p{font-size:14px}.comments{font-size:22px}.comment span{font-size:16px}table{min-width:0}}@media(min-width:2200px){.side{width:84px}.side.open{width:250px}.page{margin-left:84px}.page.open{margin-left:250px}.hour-row td{height:clamp(60px,calc((100vh - 290px)/8),88px)}}@media(max-width:1100px){.info{grid-template-columns:repeat(3,minmax(0,1fr))}.info-box{border-bottom:2px solid var(--line)}.info-box:nth-child(3n){border-right:0}.info-box:nth-last-child(-n+2){border-bottom:0}.info-box:last-child{border-right:0}}@media(max-width:700px){.page{padding:17px 11px 27px}.toolbar{flex-direction:column}.info{grid-template-columns:1fr}.info-box,.info-box:nth-child(3n){border-right:0;border-bottom:2px solid var(--line)}.info-box:last-child{border-bottom:0}.grid{grid-template-columns:1fr}.field.full{grid-column:auto}}

/* Compact laptop mode:
   fits common 14-inch screens at browser zoom 100%. */
@media (max-width:1500px), (max-height:850px){
    .side{width:58px}
    .side.open{width:200px}
    .toggle{height:54px;font-size:22px}
    .nav{gap:4px;padding:4px 7px}
    .nav a{height:38px;padding:0 9px;border-radius:8px}
    .nav i{width:27px;font-size:15px}
    .nav span{margin-left:7px;font-size:12px}

    .page{
        margin-left:58px;
        padding:6px 8px 8px;
    }

    .page.open{margin-left:200px}

    .toolbar{
        align-items:center;
        gap:8px;
        margin-bottom:5px;
    }

    .toolbar h1{font-size:19px}
    .toolbar p{display:none}

    .actions{gap:5px}
    .actions a{
        min-height:30px;
        padding:0 9px;
        border-radius:7px;
        font-size:11px;
    }

    .board-title{padding:4px 10px}
    .board-title h2{font-size:23px}

    .info-box{min-height:38px}
    .info-key{
        padding:0 6px;
        font-size:10px;
    }

    .info-value{
        padding:0 7px;
        font-size:13px;
    }

    table{min-width:820px}

    thead th{height:44px}
    .clock{width:70px;font-size:19px}
    .metric{width:110px}
    .metric .top,.diag .top{
        top:5px;
        right:6px;
    }

    .metric .bottom,.diag .bottom{
        bottom:5px;
        left:6px;
    }

    .metric .top,.metric .bottom{
        font-size:12px;
    }

    .comments{
        padding:0 10px;
        font-size:15px;
    }

    .hour-row td{
        height:clamp(38px,calc((100vh - 190px)/8),55px);
    }

    .hour{
        width:70px;
        padding:3px;
    }

    .hour small{
        display:none;
    }

    .time{
        min-width:54px;
        height:25px;
        padding:0 5px;
        font-size:13px;
    }

    .diag{
        width:110px;
        font-size:14px;
    }

    .diag .top,.diag .bottom{
        font-size:14px;
    }

    .comment{
        min-width:280px;
        padding:6px 60px 6px 9px;
    }

    .comment span{
        font-size:12px;
        line-height:1.25;
    }

    .comment em{
        right:7px;
        font-size:9px;
    }

    .status{
        margin-top:5px;
        font-size:11px;
    }

    dialog{max-width:470px}
    .modal-head{padding:14px 16px 12px}
    .modal-head h3{font-size:17px}
    .modal-head p{font-size:11px}
    .x{width:30px;height:30px}
    .hour-nav{grid-template-columns:32px minmax(0,1fr) 32px;gap:5px;padding:8px 12px}
    .hour-nav-arrow{width:32px;height:30px;border-radius:7px}
    .hour-tabs{gap:3px}
    .hour-tab{height:29px;padding:0 2px;border-radius:6px;font-size:10px}
    .modal-body{padding:14px 16px 16px}
    .grid{gap:10px}
    .field label{margin-bottom:5px;font-size:11px}
    .input{min-height:36px;padding:7px 9px;font-size:12px}
    textarea.input{min-height:72px}
    .modal-actions{margin-top:12px}
    .btn{min-height:34px;padding:0 12px;font-size:11px}
}


.server-link {
    display: inline-flex;
    align-items: center;
    gap: 7px;
    min-height: 40px;
    padding: 0 13px;
    border: 1px solid #d2e0e7;
    border-radius: 9px;
    color: var(--blue);
    background: #ffffff;
    text-decoration: none;
    font-size: 13px;
    font-weight: 700;
}

.server-link:hover {
    border-color: #8ebbd1;
    background: #f6fafc;
}

.readonly-badge {
    display: inline-flex;
    align-items: center;
    min-height: 27px;
    margin-top: 6px;
    padding: 0 9px;
    border: 1px solid #ddcda6;
    border-radius: 999px;
    color: #73551b;
    background: #fff8e8;
    font-size: 11px;
    font-weight: 800;
}

body.read-only .hour-row {
    cursor: default;
}

body.read-only .hour-row:hover,
body.read-only .hour-row:focus {
    background: #ffffff;
}

body.read-only .comment em {
    display: none;
}

@media (max-width:1500px), (max-height:850px) {
    .server-link {
        min-height: 30px;
        padding: 0 9px;
        border-radius: 7px;
        font-size: 11px;
    }

    .readonly-badge {
        min-height: 23px;
        margin-top: 3px;
        padding: 0 7px;
        font-size: 9px;
    }
}


/* Previous/next team navigation, board information and timeline. */
.nav button.nav-action {
    display: flex;
    align-items: center;
    width: 100%;
    height: 47px;
    padding: 0 14px;
    border: 0;
    border-radius: 11px;
    cursor: pointer;
    color: #5c7481;
    background: transparent;
    white-space: nowrap;
    text-align: left;
}

.nav button.nav-action:hover {
    color: var(--blue);
    background: var(--light);
}

.nav button.nav-action i {
    width: 32px;
    text-align: center;
}

.nav button.nav-action span {
    margin-left: 10px;
    opacity: 0;
    font-size: 14px;
    font-weight: 700;
    transition: .15s;
}

.side.open .nav button.nav-action span {
    opacity: 1;
}

.nav .team-navigation-link {
    position: relative;
}

.nav .team-navigation-link .arrow-left-icon {
    animation: previous-team-arrow 1.15s ease-in-out infinite;
}

.nav .team-navigation-link .arrow-right-icon {
    animation: next-team-arrow 1.15s ease-in-out infinite;
}

@keyframes previous-team-arrow {
    0%, 100% { transform: translateX(0); }
    50% { transform: translateX(-5px); }
}

@keyframes next-team-arrow {
    0%, 100% { transform: translateX(0); }
    50% { transform: translateX(5px); }
}

.board-body {
    display: grid;
    grid-template-columns: minmax(0, 1fr) 124px;
    min-width: 0;
}

.board-body .scroll {
    min-width: 0;
}

.timeline-card {
    min-width: 0;
    border-left: 2px solid var(--line);
    background: #fbfdfe;
}

.timeline-heading {
    display: flex;
    align-items: center;
    justify-content: center;
    height: 60px;
    padding: 0 8px;
    border-bottom: 2px solid var(--line);
    color: #36515f;
    font-size: 12px;
    font-weight: 800;
    text-align: center;
    text-transform: uppercase;
}

.timeline-list {
    position: relative;
    display: grid;
    grid-template-rows: repeat(8, clamp(50px, calc((100vh - 270px) / 8), 74px));
}

.timeline-list::before {
    content: "";
    position: absolute;
    top: 18px;
    bottom: 18px;
    left: 28px;
    width: 3px;
    border-radius: 999px;
    background: #d4e0e6;
}

.timeline-item {
    position: relative;
    display: flex;
    align-items: center;
    min-height: 0;
    padding: 4px 5px 4px 47px;
    border-bottom: 2px solid #e1e9ed;
}

.timeline-item:last-child {
    border-bottom: 0;
}

.timeline-marker {
    position: absolute;
    left: 17px;
    z-index: 1;
    width: 24px;
    height: 24px;
    border: 3px solid #ffffff;
    border-radius: 999px;
    background: #9fb3be;
    box-shadow: 0 0 0 1px #b9c9d1;
}

.timeline-item.completed .timeline-marker {
    background: #54a56b;
    box-shadow: 0 0 0 1px #75b788;
}

.timeline-item.scrap-event .timeline-marker {
    background: #d85e54;
    box-shadow: 0 0 0 1px #df8179;
}

.timeline-item.comment-event .timeline-marker {
    background: #e5a443;
    box-shadow: 0 0 0 1px #eabe78;
}

.timeline-content {
    min-width: 0;
}

.timeline-time {
    display: block;
    overflow: hidden;
    color: #1c658e;
    font-size: 12px;
    font-weight: 800;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.timeline-detail {
    display: block;
    overflow: hidden;
    margin-top: 2px;
    color: #6a7b85;
    font-size: 9px;
    font-weight: 700;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.info-dialog {
    max-width: 590px;
}

.board-info-grid {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 11px;
}

.board-info-item {
    min-width: 0;
    padding: 12px;
    border: 1px solid #dce7ec;
    border-radius: 10px;
    background: #f8fbfc;
}

.board-info-item.full {
    grid-column: 1 / -1;
}

.board-info-label {
    display: block;
    margin-bottom: 5px;
    color: #71828c;
    font-size: 10px;
    font-weight: 800;
    letter-spacing: .04em;
    text-transform: uppercase;
}

.board-info-value {
    display: block;
    overflow: hidden;
    color: #1d668f;
    font-size: 14px;
    font-weight: 800;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.info-dialog-actions {
    display: flex;
    justify-content: flex-end;
    margin-top: 16px;
}

@media (max-width: 1050px) {
    .board-body {
        grid-template-columns: minmax(0, 1fr) 105px;
    }

    .timeline-heading {
        font-size: 10px;
    }

    .timeline-item {
        padding-left: 42px;
    }

    .timeline-list::before {
        left: 24px;
    }

    .timeline-marker {
        left: 13px;
    }
}

@media (max-width: 820px) {
    .board-body {
        display: block;
    }

    .timeline-card {
        border-top: 2px solid var(--line);
        border-left: 0;
    }

    .timeline-heading {
        height: 38px;
    }

    .timeline-list {
        grid-template-columns: repeat(8, minmax(90px, 1fr));
        grid-template-rows: none;
        overflow-x: auto;
    }

    .timeline-list::before {
        top: 21px;
        right: 22px;
        bottom: auto;
        left: 22px;
        width: auto;
        height: 3px;
    }

    .timeline-item {
        min-height: 72px;
        padding: 35px 6px 7px;
        border-right: 1px solid #e1e9ed;
        border-bottom: 0;
        text-align: center;
    }

    .timeline-marker {
        top: 10px;
        left: 50%;
        transform: translateX(-50%);
    }
}

@media (max-width:1500px), (max-height:850px) {
    .nav button.nav-action {
        height: 38px;
        padding: 0 9px;
        border-radius: 8px;
    }

    .nav button.nav-action i {
        width: 27px;
        font-size: 15px;
    }

    .nav button.nav-action span {
        margin-left: 7px;
        font-size: 12px;
    }

    .board-body {
        grid-template-columns: minmax(0, 1fr) 104px;
    }

    .timeline-heading {
        height: 44px;
        font-size: 9px;
    }

    .timeline-list {
        grid-template-rows: repeat(8, clamp(38px, calc((100vh - 190px) / 8), 55px));
    }

    .timeline-list::before {
        left: 22px;
    }

    .timeline-item {
        padding-left: 39px;
    }

    .timeline-marker {
        left: 11px;
        width: 22px;
        height: 22px;
    }

    .timeline-time {
        font-size: 10px;
    }

    .timeline-detail {
        font-size: 8px;
    }
}


/* FR / EN navigation */
.language-navigation #languageNavText {
    overflow: hidden;
    text-overflow: ellipsis;
}

/* Hourly performance colors. */
.hour-row {
    position: relative;
    transition:
        background-color .2s ease,
        box-shadow .2s ease;
}

.hour-row td {
    transition:
        background-color .2s ease,
        border-color .2s ease;
}

.hour-row.status-pending td {
    background: #ffffff;
}

.hour-row.status-green td {
    background: #eaf8ef;
}

.hour-row.status-yellow td {
    background: #fff9d9;
}

.hour-row.status-orange td {
    background: #fff0df;
}

.hour-row.status-red td {
    background: #fde9e7;
}

.hour-row.status-green .time {
    border-color: #65ad79;
    color: #226b38;
    background: #dff3e5;
}

.hour-row.status-yellow .time {
    border-color: #d7b64c;
    color: #715b0f;
    background: #fff2b8;
}

.hour-row.status-orange .time {
    border-color: #df9145;
    color: #814a14;
    background: #ffe0bd;
}

.hour-row.status-red .time {
    border-color: #d7655d;
    color: #8f2f29;
    background: #f8d0cc;
}

.hour-row.status-green {
    box-shadow: inset 5px 0 0 #54a56b;
}

.hour-row.status-yellow {
    box-shadow: inset 5px 0 0 #d6b339;
}

.hour-row.status-orange {
    box-shadow: inset 5px 0 0 #e29443;
}

.hour-row.status-red {
    box-shadow: inset 5px 0 0 #d85e54;
}

/* Timeline uses exactly the same status colors as the table rows. */
.timeline-item.performance-pending .timeline-marker {
    background: #9fb3be;
    box-shadow: 0 0 0 1px #b9c9d1;
}

.timeline-item.performance-green .timeline-marker {
    background: #54a56b;
    box-shadow: 0 0 0 1px #75b788;
}

.timeline-item.performance-yellow .timeline-marker {
    background: #d6b339;
    box-shadow: 0 0 0 1px #e3ca72;
}

.timeline-item.performance-orange .timeline-marker {
    background: #e29443;
    box-shadow: 0 0 0 1px #e9b474;
}

.timeline-item.performance-red .timeline-marker {
    background: #d85e54;
    box-shadow: 0 0 0 1px #df8179;
}


.btn:disabled {
    cursor: wait;
    opacity: .58;
}

.modal-saving-indicator {
    display: none;
    align-items: center;
    gap: 7px;
    margin-right: auto;
    color: #5d7481;
    font-size: 12px;
    font-weight: 700;
}

.modal-saving-indicator.visible {
    display: inline-flex;
}

.modal-saving-indicator i {
    animation: modal-save-spin .8s linear infinite;
}

@keyframes modal-save-spin {
    to {
        transform: rotate(360deg);
    }
}


/* Product-rate plan and product-change workflow. */
.actions button {
    display: inline-flex;
    align-items: center;
    gap: 7px;
    min-height: 40px;
    padding: 0 13px;
    border: 1px solid #d2e0e7;
    border-radius: 9px;
    cursor: pointer;
    color: var(--blue);
    background: #ffffff;
    font-size: 13px;
    font-weight: 700;
}

.actions button:hover {
    border-color: #8ebbd1;
    background: #f6fafc;
}

.hour-product-badge {
    display: block;
    max-width: 76px;
    margin: 3px auto 0;
    overflow: hidden;
    color: #5e7480;
    font-size: 8px;
    font-weight: 800;
    line-height: 1.15;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.product-change-note {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    max-width: 100%;
    margin: 0 0 5px;
    padding: 5px 8px;
    border: 1px solid #f2cf95;
    border-radius: 8px;
    color: #7a4c0c;
    background: #fff8e8;
    font-size: 11px;
    font-weight: 800;
}

.product-change-note i {
    color: #d68716;
}

.product-change-dialog {
    max-width: 620px;
}

.product-change-summary {
    display: grid;
    grid-template-columns: repeat(3, minmax(0, 1fr));
    gap: 9px;
    margin-bottom: 15px;
}

.product-change-summary-item {
    min-width: 0;
    padding: 10px;
    border: 1px solid #dbe7ec;
    border-radius: 10px;
    background: #f8fbfc;
}

.product-change-summary-label {
    display: block;
    margin-bottom: 4px;
    color: #71828c;
    font-size: 9px;
    font-weight: 800;
    text-transform: uppercase;
}

.product-change-summary-value {
    display: block;
    overflow: hidden;
    color: #1d668f;
    font-size: 13px;
    font-weight: 800;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.product-target-preview {
    margin-top: 14px;
    padding: 12px;
    border: 1px solid #b9d8e6;
    border-radius: 10px;
    color: #24586f;
    background: #f1f9fc;
    font-size: 12px;
    line-height: 1.5;
}

.product-target-preview strong {
    color: #134f6d;
}

.product-change-warning {
    margin-top: 10px;
    color: #7a5a18;
    font-size: 11px;
    line-height: 1.45;
}

@media (max-width:1500px), (max-height:850px) {
    .actions button {
        min-height: 30px;
        padding: 0 9px;
        border-radius: 7px;
        font-size: 11px;
    }

    .hour-product-badge {
        max-width: 60px;
        margin-top: 1px;
        font-size: 7px;
    }

    .product-change-note {
        margin-bottom: 3px;
        padding: 3px 6px;
        font-size: 9px;
    }
}

@media (max-width:700px) {
    .product-change-summary {
        grid-template-columns: 1fr;
    }
}

</style>
</head>
<body>
<form id="form1" runat="server">
<asp:HiddenField
    ID="CanEditProductionBoardHiddenField"
    runat="server"
    ClientIDMode="Static"
    Value="false" />
<asp:HiddenField
    ID="CurrentBoardIdHiddenField"
    runat="server"
    ClientIDMode="Static"
    Value="0" />
<aside id="side" class="side">
<button type="button" class="toggle" onclick="toggleMenu()" title="Menu"><i class="fa-solid fa-bars"></i></button>
<nav class="nav">
<a class="active" href="PB_page.aspx" title="Production Board">
<i class="fa-solid fa-table-cells-large"></i><span data-i18n="nav.productionBoard">Production Board</span>
</a>

<button type="button" class="nav-action" onclick="openBoardInfo()" title="Tableau de marche information">
<i class="fa-solid fa-circle-info"></i><span data-i18n="nav.boardInfo">Board information</span>
</button>

<asp:HyperLink
    ID="PreviousBoardLink"
    runat="server"
    CssClass="team-navigation-link"
    ToolTip="Previous team production board"
    Visible="false">
    <i class="fa-solid fa-arrow-left-long arrow-left-icon"></i>
    <span data-i18n="nav.previousTeam">Previous team</span>
</asp:HyperLink>

<asp:HyperLink
    ID="NextBoardLink"
    runat="server"
    CssClass="team-navigation-link"
    ToolTip="Next team production board"
    Visible="false">
    <i class="fa-solid fa-arrow-right-long arrow-right-icon"></i>
    <span data-i18n="nav.nextTeam">Next team</span>
</asp:HyperLink>

<a href="WeeklySyntheses.aspx" title="Weekly synthesis">
<i class="fa-solid fa-chart-column"></i><span data-i18n="nav.weekly">Weekly Synthesis</span>
</a>


<button type="button"
        class="nav-action language-navigation app-language-trigger"
        data-app-language-trigger="true"
        title="Language">
    <i class="fa-solid fa-language" aria-hidden="true"></i>
    <span id="languageNavText"
          class="app-language-label"
          data-app-language-label="true">
        Language &middot; EN
    </span>
</button>

<a href="FindBoard.aspx" title="Find a board">
<i class="fa-solid fa-magnifying-glass"></i><span data-i18n="nav.findBoard">Find a board</span>
</a>

<a href="BoardSetup.aspx" title="Change board">
<i class="fa-solid fa-sliders"></i><span data-i18n="nav.changeBoard">Change board</span>
</a>

<a href="Logout.aspx" title="Sign out">
<i class="fa-solid fa-right-from-bracket"></i><span data-i18n="nav.signOut">Sign out</span>
</a>
</nav>
</aside>
<main id="page" class="page"><div class="wrap">
<header class="toolbar">
<div>
    <h1 data-i18n="page.title">Production Board</h1>

    <p>
        <span data-i18n="page.subtitle">Use the animated arrows in the navigation bar to move between team boards.</span>
    </p>

    <asp:Label
        ID="ReadOnlyModeLabel"
        runat="server"
        CssClass="readonly-badge"
        Visible="false" />
</div>

<div class="actions">
    <a href="FindBoard.aspx">
        <i class="fa-solid fa-magnifying-glass"></i>
        <span data-i18n="nav.findBoard">Find board</span>
    </a>

    <a href="WeeklySyntheses.aspx">
        <i class="fa-solid fa-chart-line"></i>
        <span data-i18n="nav.weekly">Weekly Synthesis</span>
    </a>

    <button id="changeProductToolbarButton"
            type="button"
            onclick="openProductChangeModal()"
            title="Change the product from a selected hour">
        <i class="fa-solid fa-arrows-rotate"></i>
        <span>Change product</span>
    </button>

    <a href="javascript:void(0)" onclick="openBoardInfo()">
        <i class="fa-solid fa-circle-info"></i>
        <span data-i18n="nav.boardInfo">Board info</span>
    </a>
</div>
</header>
<section class="board">
<div class="board-title"><h2 data-i18n="page.boardTitle">Production Board</h2></div>
<div class="info">
<div class="info-box">
<span class="info-key" data-i18n="info.date">Date</span>
<asp:Label ID="DateLabel" runat="server" ClientIDMode="Static" CssClass="info-value" Text="Date" />
</div>
<div class="info-box">
<span class="info-key" data-i18n="info.shift">Shift</span>
<asp:Label ID="ShiftLabel" runat="server" ClientIDMode="Static" CssClass="info-value" Text="Shift" />
</div>
<div class="info-box">
<span class="info-key" data-i18n="info.team">Team</span>
<asp:Label ID="TeamLabel" runat="server" ClientIDMode="Static" CssClass="info-value" Text="Team" />
</div>
<div class="info-box">
<span class="info-key" data-i18n="info.product">Product</span>
<asp:Label ID="ProductLabel" runat="server" ClientIDMode="Static" CssClass="info-value" Text="Product" />
</div>
<div class="info-box">
<span class="info-key" data-i18n="info.line">Line</span>
<asp:Label ID="PLLabel" runat="server" ClientIDMode="Static" CssClass="info-value" Text="Production line" />
</div>
</div>
<div class="board-body">
<div class="scroll"><table aria-label="Hourly production board"><thead><tr>
<th class="clock"><i class="fa-regular fa-clock"></i></th>
<th class="metric"><span class="top" data-i18n="table.target">Target</span><span class="bottom" data-i18n="table.cumulative">Cumulative</span></th>
<th class="metric"><span class="top" data-i18n="table.actual">Actual</span><span class="bottom" data-i18n="table.cumulative">Cumulative</span></th>
<th class="metric"><span class="top" data-i18n="table.scrap">Scrap</span><span class="bottom" data-i18n="table.cumulative">Cumulative</span></th>
<th class="comments" data-i18n="table.comments">Comments</th>
</tr></thead><tbody>
<tr id="hourRow1" data-hour="1" class="hour-row status-pending" onclick="openHourModal(1)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(1);}">
<td class="hour"><small>H1</small><asp:Label ID="h1Label" runat="server" CssClass="time" Text="1" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h1Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML1" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><asp:Label ID="reel_h1" runat="server" ClientIDMode="Static" Text="0" /></b><b class="bottom" id="cumul_h1">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h1">0</b><b class="bottom" id="cumulrubut_h1">0</b></td>
<td class="comment"><span id="Commentaire_h1">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow2" data-hour="2" class="hour-row status-pending" onclick="openHourModal(2)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(2);}">
<td class="hour"><small>H2</small><asp:Label ID="h2Label" runat="server" CssClass="time" Text="2" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h2Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML2" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h2">0</span></b><b class="bottom" id="cumul_h2">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h2">0</b><b class="bottom" id="cumulrubut_h2">0</b></td>
<td class="comment"><span id="Commentaire_h2">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow3" data-hour="3" class="hour-row status-pending" onclick="openHourModal(3)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(3);}">
<td class="hour"><small>H3</small><asp:Label ID="h3Label" runat="server" CssClass="time" Text="3" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h3Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML3" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h3">0</span></b><b class="bottom" id="cumul_h3">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h3">0</b><b class="bottom" id="cumulrubut_h3">0</b></td>
<td class="comment"><span id="Commentaire_h3">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow4" data-hour="4" class="hour-row status-pending" onclick="openHourModal(4)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(4);}">
<td class="hour"><small>H4</small><asp:Label ID="h4Label" runat="server" CssClass="time" Text="4" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h4Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML4" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h4">0</span></b><b class="bottom" id="cumul_h4">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h4">0</b><b class="bottom" id="cumulrubut_h4">0</b></td>
<td class="comment"><span id="Commentaire_h4">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow5" data-hour="5" class="hour-row status-pending" onclick="openHourModal(5)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(5);}">
<td class="hour"><small>H5</small><asp:Label ID="h5Label" runat="server" CssClass="time" Text="5" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h5Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML5" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h5">0</span></b><b class="bottom" id="cumul_h5">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h5">0</b><b class="bottom" id="cumulrubut_h5">0</b></td>
<td class="comment"><span id="Commentaire_h5">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow6" data-hour="6" class="hour-row status-pending" onclick="openHourModal(6)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(6);}">
<td class="hour"><small>H6</small><asp:Label ID="h6Label" runat="server" CssClass="time" Text="6" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h6Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML6" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h6">0</span></b><b class="bottom" id="cumul_h6">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h6">0</b><b class="bottom" id="cumulrubut_h6">0</b></td>
<td class="comment"><span id="Commentaire_h6">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow7" data-hour="7" class="hour-row status-pending" onclick="openHourModal(7)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(7);}">
<td class="hour"><small>H7</small><asp:Label ID="h7Label" runat="server" CssClass="time" Text="7" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h7Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML7" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h7">0</span></b><b class="bottom" id="cumul_h7">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h7">0</b><b class="bottom" id="cumulrubut_h7">0</b></td>
<td class="comment"><span id="Commentaire_h7">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
<tr id="hourRow8" data-hour="8" class="hour-row status-pending" onclick="openHourModal(8)" tabindex="0" onkeydown="if(event.key==='Enter'||event.key===' '){event.preventDefault();openHourModal(8);}">
<td class="hour"><small>H8</small><asp:Label ID="h8Label" runat="server" CssClass="time" Text="8" /></td>
<td class="diag target"><b class="top"><asp:Label ID="h8Object" runat="server" Text="0" /></b><b class="bottom"><asp:Label ID="OBJ_CML8" runat="server" Text="0" /></b></td>
<td class="diag actual"><b class="top"><span id="reel_h8">0</span></b><b class="bottom" id="cumul_h8">0</b></td>
<td class="diag scrap"><b class="top" id="rubut_h8">0</b><b class="bottom" id="cumulrubut_h8">0</b></td>
<td class="comment"><span id="Commentaire_h8">&mdash;</span><em><i class="fa-solid fa-pen"></i><b data-i18n="table.update">Update</b></em></td>
</tr>
</tbody></table></div>
<aside class="timeline-card" aria-label="Shift production timeline">
    <div class="timeline-heading">
        <span data-i18n="timeline.title">Shift timeline</span>
    </div>

    <div id="productionTimeline" class="timeline-list"></div>
</aside>
</div>
</section>
<asp:Label ID="lblCOUCOU" runat="server" CssClass="status" Text="" />
</div></main>
<dialog id="boardInfoDialog" class="info-dialog">
<div class="modal-head">
    <div>
        <h3 data-i18n="boardInfo.title">Production board information</h3>
        <p data-i18n="boardInfo.subtitle">Current production board information.</p>
    </div>

    <button type="button" class="x" onclick="closeBoardInfo()" aria-label="Close board information">
        <i class="fa-solid fa-xmark"></i>
    </button>
</div>

<div class="modal-body">
    <div class="board-info-grid">
        <div class="board-info-item">
            <span class="board-info-label" data-i18n="boardInfo.id">Board ID</span>
            <span id="infoBoardId" class="board-info-value">0</span>
        </div>

        <div class="board-info-item">
            <span class="board-info-label" data-i18n="info.date">Date</span>
            <span id="infoDate" class="board-info-value">&mdash;</span>
        </div>

        <div class="board-info-item">
            <span class="board-info-label" data-i18n="info.team">Team</span>
            <span id="infoTeam" class="board-info-value">&mdash;</span>
        </div>

        <div class="board-info-item">
            <span class="board-info-label" data-i18n="info.shift">Shift</span>
            <span id="infoShift" class="board-info-value">&mdash;</span>
        </div>

        <div class="board-info-item">
            <span class="board-info-label" data-i18n="info.product">Product</span>
            <span id="infoProduct" class="board-info-value">&mdash;</span>
        </div>

        <div class="board-info-item">
            <span class="board-info-label" data-i18n="boardInfo.productionLine">Production line</span>
            <span id="infoLine" class="board-info-value">&mdash;</span>
        </div>

        <div class="board-info-item full">
            <span class="board-info-label" data-i18n="boardInfo.accessMode">Access mode</span>
            <span id="infoAccessMode" class="board-info-value">&mdash;</span>
        </div>
    </div>

    <div class="info-dialog-actions">
        <button type="button" class="btn save" onclick="closeBoardInfo()" data-i18n="common.close">
            Close
        </button>
    </div>
</div>
</dialog>

<dialog id="productChangeDialog" class="product-change-dialog">
<div class="modal-head">
    <div>
        <h3>Change product</h3>
        <p>Choose the product and the hour from which its production rate becomes active.</p>
    </div>

    <button type="button"
            class="x"
            onclick="closeProductChangeModal()"
            aria-label="Close product change">
        <i class="fa-solid fa-xmark"></i>
    </button>
</div>

<div class="modal-body">
    <div class="product-change-summary">
        <div class="product-change-summary-item">
            <span class="product-change-summary-label">Current product</span>
            <span id="currentProductAtHour" class="product-change-summary-value">&mdash;</span>
        </div>

        <div class="product-change-summary-item">
            <span class="product-change-summary-label">Current rate</span>
            <span id="currentRateAtHour" class="product-change-summary-value">&mdash;</span>
        </div>

        <div class="product-change-summary-item">
            <span class="product-change-summary-label">Planned stop</span>
            <span id="plannedStopAtHour" class="product-change-summary-value">&mdash;</span>
        </div>
    </div>

    <div class="grid">
        <div class="field">
            <label for="newProductSelect">New product</label>
            <select id="newProductSelect" class="input"></select>
        </div>

        <div class="field">
            <label for="effectiveHourSelect">Effective from</label>
            <select id="effectiveHourSelect" class="input">
                <option value="1">H1</option>
                <option value="2">H2</option>
                <option value="3">H3</option>
                <option value="4">H4</option>
                <option value="5">H5</option>
                <option value="6">H6</option>
                <option value="7">H7</option>
                <option value="8">H8</option>
            </select>
        </div>

        <div class="field">
            <label for="changeoverMinutesInput">Changeover duration (minutes)</label>
            <input id="changeoverMinutesInput"
                   class="input"
                   type="number"
                   min="0"
                   max="60"
                   step="1"
                   value="0" />
        </div>

        <div class="field">
            <label for="newProductRateDisplay">Selected rate</label>
            <input id="newProductRateDisplay"
                   class="input"
                   type="text"
                   value=""
                   readonly="readonly" />
        </div>

        <div class="field full">
            <label for="productChangeReason">Reason / note</label>
            <textarea id="productChangeReason"
                      class="input"
                      maxlength="500"
                      placeholder="Example: Product 3 completed; start Product 1."></textarea>
        </div>
    </div>

    <div id="productTargetPreview"
         class="product-target-preview">
        Select a product to preview the new objective.
    </div>

    <div class="product-change-warning">
        The selected product applies from the chosen hour until another product change is recorded.
        Actual production and scrap already entered are never deleted.
    </div>

    <div id="productChangeError" class="error"></div>
    <div id="productChangeSuccess" class="success"></div>

    <div class="modal-actions">
        <button type="button"
                class="btn cancel"
                onclick="closeProductChangeModal()">
            Cancel
        </button>

        <button id="confirmProductChangeButton"
                type="button"
                class="btn save"
                onclick="submitProductChange()">
            Confirm product change
        </button>
    </div>
</div>
</dialog>

<dialog id="hourModal">
<div class="modal-head">
<div><h3 id="modalTitle">Update production hour</h3><p data-i18n="modal.subtitle">Move between H1 and H8 without closing the modal.</p></div>
<button type="button" class="x" onclick="closeHourModal()" aria-label="Close"><i class="fa-solid fa-xmark"></i></button>
</div>

<div class="hour-nav" aria-label="Choose an hour">
<button id="previousHourButton" type="button" class="hour-nav-arrow" onclick="navigateHour(-1)" aria-label="Previous hour">
<i class="fa-solid fa-chevron-left"></i>
</button>

<div id="hourTabs" class="hour-tabs">
<button type="button" class="hour-tab" data-hour="1" onclick="goToHour(1)">H1</button>
<button type="button" class="hour-tab" data-hour="2" onclick="goToHour(2)">H2</button>
<button type="button" class="hour-tab" data-hour="3" onclick="goToHour(3)">H3</button>
<button type="button" class="hour-tab" data-hour="4" onclick="goToHour(4)">H4</button>
<button type="button" class="hour-tab" data-hour="5" onclick="goToHour(5)">H5</button>
<button type="button" class="hour-tab" data-hour="6" onclick="goToHour(6)">H6</button>
<button type="button" class="hour-tab" data-hour="7" onclick="goToHour(7)">H7</button>
<button type="button" class="hour-tab" data-hour="8" onclick="goToHour(8)">H8</button>
</div>

<button id="nextHourButton" type="button" class="hour-nav-arrow" onclick="navigateHour(1)" aria-label="Next hour">
<i class="fa-solid fa-chevron-right"></i>
</button>
</div>

<div class="modal-body">
<div class="grid">
<div class="field"><label for="modalActual" data-i18n="modal.actualQuantity">Actual quantity</label><input id="modalActual" class="input" type="number" min="0" step="1" placeholder="0" /></div>
<div class="field"><label for="modalScrap" data-i18n="modal.scrapQuantity">Scrap quantity</label><input id="modalScrap" class="input" type="number" min="0" step="1" placeholder="0" /></div>
<div class="field full"><label for="modalComment" data-i18n="table.comments">Comments</label><textarea id="modalComment" class="input" maxlength="1000" placeholder="Describe a stop, incident or observation..."></textarea></div>
</div>

<div id="modalError" class="error"></div>
<div id="modalSuccess" class="success"></div>

<div class="modal-actions">
<span id="modalSavingIndicator"
      class="modal-saving-indicator">
    <i class="fa-solid fa-spinner"></i>
    <span>Saving...</span>
</span>

<button id="saveHourButton"
        type="button"
        class="btn save-stay"
        onclick="saveHourUpdates(false)"
        data-i18n="common.save">
    Save
</button>

<button id="saveAndCloseHourButton"
        type="button"
        class="btn save"
        onclick="saveHourUpdates(true)"
        data-i18n="common.saveClose">
    Save &amp; Close
</button>
</div>
</div>
</dialog>
</form>
<script src="Scripts/app-language.js"></script>
<script>
    (function (window, document) {
        "use strict";

        var flagMarkup = {
            en:
                '<svg viewBox="0 0 60 40" role="img" aria-label="United Kingdom flag" ' +
                'xmlns="http://www.w3.org/2000/svg">' +
                '<rect width="60" height="40" fill="#012169"></rect>' +
                '<path d="M0 0 L60 40 M60 0 L0 40" stroke="#ffffff" stroke-width="8"></path>' +
                '<path d="M0 0 L60 40 M60 0 L0 40" stroke="#C8102E" stroke-width="4"></path>' +
                '<path d="M30 0 V40 M0 20 H60" stroke="#ffffff" stroke-width="12"></path>' +
                '<path d="M30 0 V40 M0 20 H60" stroke="#C8102E" stroke-width="7"></path>' +
                '</svg>',

            fr:
                '<svg viewBox="0 0 60 40" role="img" aria-label="French flag" ' +
                'xmlns="http://www.w3.org/2000/svg">' +
                '<rect width="20" height="40" x="0" fill="#0055A4"></rect>' +
                '<rect width="20" height="40" x="20" fill="#ffffff"></rect>' +
                '<rect width="20" height="40" x="40" fill="#EF4135"></rect>' +
                '</svg>'
        };

        function ensureStyles() {
            if (document.getElementById(
                "appLanguageFlagStyles")) {
                return;
            }

            var style =
                document.createElement("style");

            style.id =
                "appLanguageFlagStyles";

            style.textContent =
                ".app-language-code.app-language-flag-code{" +
                "width:48px;height:34px;padding:0;overflow:hidden;" +
                "border:1px solid #d5e0e6;border-radius:8px;" +
                "background:#fff;box-shadow:0 3px 10px rgba(20,65,90,.10);" +
                "}" +
                ".app-language-code.app-language-flag-code svg{" +
                "display:block;width:100%;height:100%;object-fit:cover;" +
                "}" +
                "@media(max-width:1500px),(max-height:850px){" +
                ".app-language-code.app-language-flag-code{" +
                "width:44px;height:31px;" +
                "}" +
                "}";

            document.head.appendChild(style);
        }

        function decorateOption(language) {
            var option =
                document.querySelector(
                    '[data-app-language-option="' +
                    language +
                    '"]'
                );

            if (!option) {
                return;
            }

            var code =
                option.querySelector(
                    ".app-language-code"
                );

            if (!code) {
                return;
            }

            code.classList.add(
                "app-language-flag-code"
            );

            if (code.getAttribute(
                "data-flag-language") !== language) {
                code.innerHTML =
                    flagMarkup[language];

                code.setAttribute(
                    "data-flag-language",
                    language
                );
            }
        }

        function decorateLanguageUi() {
            ensureStyles();
            decorateOption("en");
            decorateOption("fr");
        }

        function scheduleDecoration() {
            window.setTimeout(
                decorateLanguageUi,
                0
            );

            window.setTimeout(
                decorateLanguageUi,
                100
            );
        }

        if (document.readyState === "loading") {
            document.addEventListener(
                "DOMContentLoaded",
                scheduleDecoration
            );
        } else {
            scheduleDecoration();
        }

        window.addEventListener(
            "appLanguageChanged",
            scheduleDecoration
        );

        document.addEventListener(
            "click",
            function (event) {
                var trigger =
                    event.target.closest(
                        "[data-app-language-trigger]"
                    );

                if (trigger) {
                    window.setTimeout(
                        decorateLanguageUi,
                        0
                    );

                    window.setTimeout(
                        decorateLanguageUi,
                        80
                    );
                }
            }
        );
    })(window, document);
</script>
<script>
    (function () {
        "use strict";

        var activeHour = 0;
        var drafts = {};
        var dirtyHours = {};

        var modal = document.getElementById("hourModal");
        var title = document.getElementById("modalTitle");
        var actual = document.getElementById("modalActual");
        var scrap = document.getElementById("modalScrap");
        var comment = document.getElementById("modalComment");
        var error = document.getElementById("modalError");
        var success = document.getElementById("modalSuccess");
        var previousButton = document.getElementById("previousHourButton");
        var nextButton = document.getElementById("nextHourButton");
        var boardInfoDialog = document.getElementById("boardInfoDialog");
        var productionTimeline = document.getElementById("productionTimeline");
        var saveHourButton = document.getElementById("saveHourButton");
        var saveAndCloseHourButton = document.getElementById("saveAndCloseHourButton");
        var modalSavingIndicator = document.getElementById("modalSavingIndicator");
        var saveInProgress = false;

        var translations = {
            en: {
                "document.title": "Production Board",
                "nav.productionBoard": "Production Board",
                "nav.boardInfo": "Board information",
                "nav.previousTeam": "Previous team",
                "nav.nextTeam": "Next team",
                "nav.weekly": "Weekly Synthesis",
                "nav.findBoard": "Find a board",
                "nav.changeBoard": "Change board",
                "nav.signOut": "Sign out",
                "nav.language": "Language",
                "page.title": "Production Board",
                "page.subtitle": "Use the animated arrows in the navigation bar to move between team boards.",
                "page.boardTitle": "Production Board",
                "info.date": "Date",
                "info.shift": "Shift",
                "info.team": "Team",
                "info.product": "Product",
                "info.line": "Line",
                "product.mixed": "Mixed production",
                "table.target": "Target",
                "table.cumulative": "Cumulative",
                "table.actual": "Actual",
                "table.scrap": "Scrap",
                "table.comments": "Comments",
                "table.update": "Update",
                "timeline.title": "Shift timeline",
                "timeline.pending": "Pending",
                "timeline.actual": "Actual",
                "timeline.scrap": "Scrap",
                "boardInfo.title": "Production board information",
                "boardInfo.subtitle": "Current production board information.",
                "boardInfo.id": "Board ID",
                "boardInfo.productionLine": "Production line",
                "boardInfo.accessMode": "Access mode",
                "boardInfo.editable": "Editable board",
                "boardInfo.readOnly": "Read-only board",
                "modal.title": "Update production hour",
                "modal.subtitle": "Move between H1 and H8 without closing the modal.",
                "modal.actualQuantity": "Actual quantity",
                "modal.scrapQuantity": "Scrap quantity",
                "modal.commentPlaceholder": "Describe a stop, incident or observation...",
                "modal.noHour": "No production hour is selected.",
                "modal.invalidActual": "Actual quantity must be a whole number equal to or greater than zero.",
                "modal.invalidScrap": "Scrap quantity must be a whole number equal to or greater than zero.",
                "modal.saved": "Changes were saved to the database. You can continue through the other hours.",
                "modal.saveError": "The production data could not be saved to the database.",
                "common.close": "Close",
                "common.save": "Save",
                "common.saveClose": "Save & Close"
            },
            fr: {
                "document.title": "Tableau de marche",
                "nav.productionBoard": "Tableau de marche",
                "nav.boardInfo": "Informations du tableau",
                "nav.previousTeam": "\u00C9quipe pr\u00E9c\u00E9dente",
                "nav.nextTeam": "\u00C9quipe suivante",
                "nav.weekly": "Synth\u00E8se hebdomadaire",
                "nav.findBoard": "Rechercher un tableau",
                "nav.changeBoard": "Changer de tableau",
                "nav.signOut": "D\u00E9connexion",
                "nav.language": "Langue",
                "page.title": "Tableau de marche",
                "page.subtitle": "Utilisez les fl\u00E8ches anim\u00E9es dans la barre de navigation pour passer entre les tableaux des \u00E9quipes.",
                "page.boardTitle": "Tableau de marche",
                "info.date": "Date",
                "info.shift": "Poste",
                "info.team": "\u00C9quipe",
                "info.product": "Produit",
                "info.line": "Ligne",
                "product.mixed": "Production mixte",
                "table.target": "Objectif",
                "table.cumulative": "Cumul",
                "table.actual": "R\u00E9el",
                "table.scrap": "Rebut",
                "table.comments": "Commentaires",
                "table.update": "Modifier",
                "timeline.title": "Chronologie du poste",
                "timeline.pending": "En attente",
                "timeline.actual": "R\u00E9el",
                "timeline.scrap": "Rebut",
                "boardInfo.title": "Informations du tableau de marche",
                "boardInfo.subtitle": "Informations du tableau de production actuel.",
                "boardInfo.id": "ID du tableau",
                "boardInfo.productionLine": "Ligne de production",
                "boardInfo.accessMode": "Mode d\u2019acc\u00E8s",
                "boardInfo.editable": "Tableau modifiable",
                "boardInfo.readOnly": "Tableau en lecture seule",
                "modal.title": "Mise \u00E0 jour de l\u2019heure de production",
                "modal.subtitle": "Passez de H1 \u00E0 H8 sans fermer la fen\u00EAtre.",
                "modal.actualQuantity": "Quantit\u00E9 r\u00E9elle",
                "modal.scrapQuantity": "Quantit\u00E9 rebut",
                "modal.commentPlaceholder": "D\u00E9crivez un arr\u00EAt, un incident ou une observation...",
                "modal.noHour": "Aucune heure de production n\u2019est s\u00E9lectionn\u00E9e.",
                "modal.invalidActual": "La quantit\u00E9 r\u00E9elle doit \u00EAtre un nombre entier sup\u00E9rieur ou \u00E9gal \u00E0 z\u00E9ro.",
                "modal.invalidScrap": "La quantit\u00E9 rebut doit \u00EAtre un nombre entier sup\u00E9rieur ou \u00E9gal \u00E0 z\u00E9ro.",
                "modal.saved": "Les modifications ont \u00E9t\u00E9 enregistr\u00E9es dans la base de donn\u00E9es. Vous pouvez continuer avec les autres heures.",
                "modal.saveError": "Les donn\u00E9es de production n\u2019ont pas pu \u00EAtre enregistr\u00E9es dans la base de donn\u00E9es.",
                "common.close": "Fermer",
                "common.save": "Enregistrer",
                "common.saveClose": "Enregistrer et fermer"
            }
        };

        var currentLanguage =
            readStoredLanguage();

        function readStoredLanguage() {
            try {
                var storedLanguage =
                    window.localStorage.getItem(
                        "productionBoardLanguage"
                    );

                return storedLanguage === "fr"
                    ? "fr"
                    : "en";
            } catch (exception) {
                return "en";
            }
        }

        function storeLanguage(language) {
            try {
                window.localStorage.setItem(
                    "productionBoardLanguage",
                    language
                );
            } catch (exception) {
                /*
                 * The page still works when browser storage is unavailable.
                 */
            }
        }

        function t(key) {
            var languageDictionary =
                translations[currentLanguage] ||
                translations.en;

            return languageDictionary[key] ||
                translations.en[key] ||
                key;
        }

        function applyLanguage() {
            document.documentElement.lang =
                currentLanguage;

            document.title =
                t("document.title");

            var translatedElements =
                document.querySelectorAll(
                    "[data-i18n]"
                );

            for (var index = 0;
                index < translatedElements.length;
                index++) {

                var element =
                    translatedElements[index];

                var key =
                    element.getAttribute(
                        "data-i18n"
                    );

                element.textContent =
                    t(key);
            }

            var languageText =
                document.getElementById(
                    "languageNavText"
                );

            if (languageText) {
                languageText.textContent =
                    t("nav.language") +
                    " \u00B7 " +
                    currentLanguage.toUpperCase();
            }

            var productLabel =
                document.getElementById(
                    "ProductLabel"
                );

            var productPlan =
                window.productionBoardProductPlan;

            var isMixedProduction =
                productLabel &&
                (
                    productLabel.getAttribute(
                        "data-product-mode"
                    ) === "mixed"
                    ||
                    (
                        productPlan &&
                        productPlan.IsMixed === true
                    )
                );

            if (isMixedProduction) {
                productLabel.textContent =
                    t("product.mixed");
            }

            if (comment) {
                comment.placeholder =
                    t("modal.commentPlaceholder");
            }

            if (activeHour > 0) {
                loadHourIntoModal(activeHour);
            }

            renderProductionTimeline(
                readTimelineHoursFromBoard()
            );
        }

        /*
         * The shared app-language.js file owns the language selector modal.
         * This page listens for the global change event so its timeline,
         * dynamic modal titles and validation messages also update.
         */
        window.toggleLanguage = function () {
            if (window.AppLanguage) {
                window.AppLanguage.openModal();
            }
        };

        window.addEventListener(
            "appLanguageChanged",
            function (event) {
                if (!event.detail ||
                    !event.detail.language) {
                    return;
                }

                currentLanguage =
                    event.detail.language;

                applyLanguage();
            }
        );

        var canEditField =
            document.getElementById(
                "CanEditProductionBoardHiddenField"
            );

        var canEdit =
            canEditField !== null &&
            canEditField.value === "true";

        if (!canEdit) {
            document.body.classList.add("read-only");
        }

        window.toggleMenu = function () {
            document.getElementById("side").classList.toggle("open");
            document.getElementById("page").classList.toggle("open");
        };

        window.openBoardInfo = function () {
            setInfoValue("infoBoardId", getElementValue("CurrentBoardIdHiddenField"));
            setInfoValue("infoDate", getElementValue("DateLabel"));
            setInfoValue("infoTeam", getElementValue("TeamLabel"));
            setInfoValue("infoShift", getElementValue("ShiftLabel"));
            setInfoValue("infoProduct", getElementValue("ProductLabel"));
            setInfoValue("infoLine", getElementValue("PLLabel"));
            setInfoValue(
                "infoAccessMode",
                canEdit
                    ? t("boardInfo.editable")
                    : t("boardInfo.readOnly")
            );

            if (typeof boardInfoDialog.showModal === "function") {
                boardInfoDialog.showModal();
            } else {
                boardInfoDialog.setAttribute("open", "open");
            }
        };

        window.closeBoardInfo = function () {
            if (typeof boardInfoDialog.close === "function") {
                boardInfoDialog.close();
            } else {
                boardInfoDialog.removeAttribute("open");
            }
        };

        function getElementValue(elementId) {
            var element = document.getElementById(elementId);

            if (!element) {
                return "\u2014";
            }

            var value = "value" in element
                ? element.value
                : element.textContent;

            value = String(value || "").trim();

            return value || "\u2014";
        }

        function setInfoValue(elementId, value) {
            var element = document.getElementById(elementId);

            if (element) {
                element.textContent = value || "\u2014";
            }
        }

        function getHourPerformanceStatus(
            targetQuantity,
            actualQuantity,
            scrapQuantity) {

            var targetValue =
                Math.max(0, Number(targetQuantity || 0));

            var actualValue =
                Math.max(0, Number(actualQuantity || 0));

            var scrapValue =
                Math.max(0, Number(scrapQuantity || 0));

            /*
             * Pending:
             * No production and no scrap have been entered yet.
             */
            if (actualValue === 0 && scrapValue === 0) {
                return {
                    name: "pending",
                    ratio: targetValue > 0
                        ? 0
                        : null
                };
            }

            /*
             * Any scrap marks the hour red.
             */
            if (scrapValue > 0) {
                return {
                    name: "red",
                    ratio: targetValue > 0
                        ? actualValue / targetValue
                        : null
                };
            }

            /*
             * When no target exists, a positive actual value is considered green.
             */
            if (targetValue <= 0) {
                return {
                    name: actualValue > 0
                        ? "green"
                        : "pending",
                    ratio: null
                };
            }

            var ratio =
                actualValue / targetValue;

            if (ratio >= 1) {
                return {
                    name: "green",
                    ratio: ratio
                };
            }

            if (ratio >= 0.90) {
                return {
                    name: "yellow",
                    ratio: ratio
                };
            }

            if (ratio >= 0.50) {
                return {
                    name: "orange",
                    ratio: ratio
                };
            }

            return {
                name: "red",
                ratio: ratio
            };
        }

        function getPerformanceDetail(
            status,
            targetQuantity,
            actualQuantity,
            scrapQuantity) {

            var targetValue =
                Math.max(0, Number(targetQuantity || 0));

            var actualValue =
                Math.max(0, Number(actualQuantity || 0));

            var scrapValue =
                Math.max(0, Number(scrapQuantity || 0));

            if (status.name === "pending") {
                return t("timeline.pending");
            }

            if (scrapValue > 0) {
                return t("timeline.scrap") +
                    " " +
                    scrapValue;
            }

            if (targetValue <= 0) {
                return t("timeline.actual") +
                    " " +
                    actualValue;
            }

            var percentage =
                Math.round(
                    (status.ratio || 0) * 100
                );

            return percentage +
                "% \u00B7 " +
                actualValue +
                "/" +
                targetValue;
        }

        function applyHourPerformanceColors() {
            for (var hourNumber = 1;
                hourNumber <= 8;
                hourNumber++) {

                var row =
                    document.getElementById(
                        "hourRow" + hourNumber
                    );

                if (!row) {
                    continue;
                }

                var targetQuantity =
                    read(
                        document.getElementById(
                            "h" + hourNumber + "Object"
                        )
                    );

                var actualQuantity =
                    read(
                        document.getElementById(
                            "reel_h" + hourNumber
                        )
                    );

                var scrapQuantity =
                    read(
                        document.getElementById(
                            "rubut_h" + hourNumber
                        )
                    );

                var status =
                    getHourPerformanceStatus(
                        targetQuantity,
                        actualQuantity,
                        scrapQuantity
                    );

                row.classList.remove(
                    "status-pending",
                    "status-green",
                    "status-yellow",
                    "status-orange",
                    "status-red"
                );

                row.classList.add(
                    "status-" + status.name
                );

                row.setAttribute(
                    "data-performance-status",
                    status.name
                );
            }
        }

        window.renderProductionTimeline = function (hours) {
            if (!productionTimeline) {
                return;
            }

            productionTimeline.innerHTML = "";

            var items =
                Array.isArray(hours)
                    ? hours
                    : readTimelineHoursFromBoard();

            for (var index = 0;
                index < 8;
                index++) {

                var hour =
                    items[index] || {
                        hourNumber: index + 1,
                        hourLabel: "H" + (index + 1),
                        targetQuantity: 0,
                        actualQuantity: 0,
                        scrapQuantity: 0,
                        comment: ""
                    };

                var hourNumber =
                    Number(hour.hourNumber || (index + 1));

                var targetQuantity =
                    hour.targetQuantity !== undefined
                        ? Number(hour.targetQuantity || 0)
                        : read(
                            document.getElementById(
                                "h" + hourNumber + "Object"
                            )
                        );

                var actualValue =
                    Number(hour.actualQuantity || 0);

                var scrapValue =
                    Number(hour.scrapQuantity || 0);

                var status =
                    getHourPerformanceStatus(
                        targetQuantity,
                        actualValue,
                        scrapValue
                    );

                var item =
                    document.createElement("div");

                item.className =
                    "timeline-item performance-" +
                    status.name;

                var marker =
                    document.createElement("span");

                marker.className =
                    "timeline-marker";

                var content =
                    document.createElement("span");

                content.className =
                    "timeline-content";

                var time =
                    document.createElement("span");

                time.className =
                    "timeline-time";

                time.textContent =
                    hour.hourLabel ||
                    ("H" + (index + 1));

                var detail =
                    document.createElement("span");

                detail.className =
                    "timeline-detail";

                detail.textContent =
                    getPerformanceDetail(
                        status,
                        targetQuantity,
                        actualValue,
                        scrapValue
                    );

                content.appendChild(time);
                content.appendChild(detail);
                item.appendChild(marker);
                item.appendChild(content);
                productionTimeline.appendChild(item);
            }

            applyHourPerformanceColors();
        };

        function readTimelineHoursFromBoard() {
            var hours = [];

            for (var hourNumber = 1; hourNumber <= 8; hourNumber++) {
                var timeElement = document.getElementById("h" + hourNumber + "Label");
                var commentElement = document.getElementById("Commentaire_h" + hourNumber);

                hours.push({
                    hourNumber: hourNumber,
                    hourLabel: timeElement
                        ? String(timeElement.textContent || "").trim()
                        : "H" + hourNumber,
                    targetQuantity: read(
                        document.getElementById(
                            "h" + hourNumber + "Object"
                        )
                    ),
                    actualQuantity: read(
                        document.getElementById(
                            "reel_h" + hourNumber
                        )
                    ),
                    scrapQuantity: read(
                        document.getElementById(
                            "rubut_h" + hourNumber
                        )
                    ),
                    comment: commentElement
                        ? String(commentElement.textContent || "").trim()
                        : ""
                });
            }

            return hours;
        }

        window.openHourModal = function (hourNumber) {
            if (!canEdit) {
                return;
            }

            drafts = {};
            dirtyHours = {};

            for (var number = 1; number <= 8; number++) {
                drafts[number] = readHourFromBoard(number);
            }

            activeHour = hourNumber;
            loadHourIntoModal(activeHour);
            updateNavigation();

            hideMessages();

            if (typeof modal.showModal === "function") {
                modal.showModal();
            } else {
                modal.setAttribute("open", "open");
            }

            window.setTimeout(function () {
                actual.focus();
                actual.select();
            }, 25);
        };

        window.goToHour = function (hourNumber) {
            if (hourNumber < 1 || hourNumber > 8 || hourNumber === activeHour) {
                return;
            }

            if (!storeActiveHourDraft()) {
                return;
            }

            activeHour = hourNumber;
            loadHourIntoModal(activeHour);
            updateNavigation();
            hideMessages();

            actual.focus();
            actual.select();
        };

        window.navigateHour = function (direction) {
            var targetHour = activeHour + direction;

            if (targetHour < 1 || targetHour > 8) {
                return;
            }

            window.goToHour(targetHour);
        };

        window.closeHourModal = function () {
            if (saveInProgress) {
                return;
            }

            hideMessages();

            if (typeof modal.close === "function") {
                modal.close();
            } else {
                modal.removeAttribute("open");
            }

            activeHour = 0;
            drafts = {};
            dirtyHours = {};
        };

        window.saveHourUpdates = function (closeAfterSave) {
            if (saveInProgress) {
                return;
            }

            hideMessages();

            if (!storeActiveHourDraft()) {
                return;
            }

            var saveRequest =
                buildProductionHoursSaveRequest();

            if (!saveRequest) {
                showError(
                    t("modal.saveError")
                );

                return;
            }

            setSavingState(true);

            saveProductionHoursToServer(
                saveRequest
            )
                .then(function (result) {
                    if (!result ||
                        result.Success !== true) {
                        throw new Error(
                            result && result.Message
                                ? result.Message
                                : t("modal.saveError")
                        );
                    }

                    applyDraftsToBoard();
                    recalculateCumulativeValues();
                    renderProductionTimeline(
                        readTimelineHoursFromBoard()
                    );
                    applyHourPerformanceColors();

                    window.productionBoardHours =
                        readTimelineHoursFromBoard();

                    dirtyHours = {};
                    updateNavigation();

                    if (closeAfterSave) {
                        setSavingState(false);
                        closeHourModal();
                        return;
                    }

                    showSuccess(
                        t("modal.saved")
                    );
                })
                .catch(function (saveError) {
                    showError(
                        saveError && saveError.message
                            ? saveError.message
                            : t("modal.saveError")
                    );
                })
                .then(function () {
                    setSavingState(false);
                });
        };

        function buildProductionHoursSaveRequest() {
            var boardIdElement =
                document.getElementById(
                    "CurrentBoardIdHiddenField"
                );

            var boardId =
                boardIdElement
                    ? parseInt(
                        boardIdElement.value,
                        10
                    )
                    : 0;

            if (!Number.isFinite(boardId) ||
                boardId <= 0) {
                return null;
            }

            var hours = [];

            for (var hourNumber = 1;
                hourNumber <= 8;
                hourNumber++) {

                var draft =
                    drafts[hourNumber] ||
                    readHourFromBoard(
                        hourNumber
                    );

                hours.push({
                    HourNumber:
                        hourNumber,

                    ActualQuantity:
                        Number(draft.actual || 0),

                    ScrapQuantity:
                        Number(draft.scrap || 0),

                    Comment:
                        String(draft.comment || "")
                            .trim()
                });
            }

            return {
                BoardId:
                    boardId,

                Hours:
                    hours
            };
        }

        function saveProductionHoursToServer(
            request) {

            var endpoint =
                window.location.pathname +
                "/SaveProductionHours";

            return window.fetch(
                endpoint,
                {
                    method: "POST",
                    credentials: "same-origin",
                    headers: {
                        "Content-Type":
                            "application/json; charset=utf-8",

                        "Accept":
                            "application/json"
                    },
                    body: JSON.stringify({
                        request: request
                    })
                }
            )
                .then(function (response) {
                    return response.text()
                        .then(function (responseText) {
                            var payload = null;

                            try {
                                payload =
                                    responseText
                                        ? JSON.parse(
                                            responseText
                                        )
                                        : null;
                            } catch (parseError) {
                                payload = null;
                            }

                            if (!response.ok) {
                                var serverMessage =
                                    payload && payload.Message
                                        ? payload.Message
                                        : t("modal.saveError");

                                throw new Error(
                                    serverMessage
                                );
                            }

                            var result =
                                payload && payload.d !== undefined
                                    ? payload.d
                                    : payload;

                            if (typeof result === "string") {
                                try {
                                    result =
                                        JSON.parse(result);
                                } catch (parseResultError) {
                                    result = null;
                                }
                            }

                            return result;
                        });
                });
        }

        function setSavingState(isSaving) {
            saveInProgress =
                isSaving === true;

            if (saveHourButton) {
                saveHourButton.disabled =
                    saveInProgress;
            }

            if (saveAndCloseHourButton) {
                saveAndCloseHourButton.disabled =
                    saveInProgress;
            }

            if (previousButton) {
                previousButton.disabled =
                    saveInProgress ||
                    activeHour <= 1;
            }

            if (nextButton) {
                nextButton.disabled =
                    saveInProgress ||
                    activeHour >= 8;
            }

            if (modalSavingIndicator) {
                modalSavingIndicator.classList.toggle(
                    "visible",
                    saveInProgress
                );
            }
        }

        /*
         * Compatibility aliases for older modal calls.
         */
        window.saveAllHourUpdates = function () {
            window.saveHourUpdates(true);
        };

        window.saveHourUpdate = function () {
            window.saveHourUpdates(false);
        };

        function applyDraftsToBoard() {
            for (var hourNumber = 1; hourNumber <= 8; hourNumber++) {
                var draft = drafts[hourNumber];

                set(
                    "reel_h" + hourNumber,
                    draft.actual
                );

                set(
                    "rubut_h" + hourNumber,
                    draft.scrap
                );

                set(
                    "Commentaire_h" + hourNumber,
                    draft.comment || "\u2014"
                );
            }
        }

        function readHourFromBoard(hourNumber) {
            var commentElement =
                document.getElementById("Commentaire_h" + hourNumber);

            var commentText =
                commentElement && commentElement.textContent
                    ? commentElement.textContent.trim()
                    : "";

            return {
                actual: read(
                    document.getElementById("reel_h" + hourNumber)
                ),
                scrap: read(
                    document.getElementById("rubut_h" + hourNumber)
                ),
                comment:
                    commentText === "\u2014" ||
                        commentText === "No comment"
                        ? ""
                        : commentText
            };
        }

        function loadHourIntoModal(hourNumber) {
            var draft = drafts[hourNumber];
            var timeElement =
                document.getElementById("h" + hourNumber + "Label");

            var timeText =
                timeElement && timeElement.textContent
                    ? timeElement.textContent.trim()
                    : "H" + hourNumber;

            title.textContent =
                t("modal.title") +
                " \u00B7 H" +
                hourNumber +
                "/8 \u2014 " +
                timeText;

            actual.value = draft.actual;
            scrap.value = draft.scrap;
            comment.value = draft.comment;
        }

        function storeActiveHourDraft() {
            if (activeHour < 1 || activeHour > 8) {
                showError(t("modal.noHour"));
                return false;
            }

            var actualValue = parseNonNegativeInteger(actual.value);
            var scrapValue = parseNonNegativeInteger(scrap.value);

            if (actualValue === null) {
                showError(
                    t("modal.invalidActual")
                );
                actual.focus();
                return false;
            }

            if (scrapValue === null) {
                showError(
                    t("modal.invalidScrap")
                );
                scrap.focus();
                return false;
            }

            var previousDraft = drafts[activeHour];
            var newComment = comment.value.trim();

            var hasChanged =
                previousDraft.actual !== actualValue ||
                previousDraft.scrap !== scrapValue ||
                previousDraft.comment !== newComment;

            drafts[activeHour] = {
                actual: actualValue,
                scrap: scrapValue,
                comment: newComment
            };

            if (hasChanged) {
                dirtyHours[activeHour] = true;
            }

            updateNavigation();
            return true;
        }

        function updateNavigation() {
            previousButton.disabled =
                saveInProgress ||
                activeHour <= 1;

            nextButton.disabled =
                saveInProgress ||
                activeHour >= 8;

            var tabs =
                document.querySelectorAll(
                    "#hourTabs .hour-tab"
                );

            for (var index = 0; index < tabs.length; index++) {
                var tab = tabs[index];
                var tabHour =
                    parseInt(
                        tab.getAttribute("data-hour"),
                        10
                    );

                tab.classList.toggle(
                    "active",
                    tabHour === activeHour
                );

                tab.classList.toggle(
                    "dirty",
                    dirtyHours[tabHour] === true
                );

                tab.setAttribute(
                    "aria-current",
                    tabHour === activeHour
                        ? "true"
                        : "false"
                );
            }
        }

        function recalculateCumulativeValues() {
            var actualCumulative = 0;
            var scrapCumulative = 0;

            for (var hourNumber = 1;
                hourNumber <= 8;
                hourNumber++) {
                actualCumulative += read(
                    document.getElementById(
                        "reel_h" + hourNumber
                    )
                );

                scrapCumulative += read(
                    document.getElementById(
                        "rubut_h" + hourNumber
                    )
                );

                set(
                    "cumul_h" + hourNumber,
                    actualCumulative
                );

                set(
                    "cumulrubut_h" + hourNumber,
                    scrapCumulative
                );
            }
        }

        function parseNonNegativeInteger(value) {
            if (value === null ||
                value === undefined ||
                String(value).trim() === "") {
                return 0;
            }

            var parsed = Number(value);

            return Number.isFinite(parsed) &&
                parsed >= 0 &&
                Math.floor(parsed) === parsed
                ? parsed
                : null;
        }

        function read(element) {
            if (!element) {
                return 0;
            }

            var parsed =
                parseInt(
                    String(element.textContent || "")
                        .replace(/[^0-9-]/g, ""),
                    10
                );

            return Number.isFinite(parsed) &&
                parsed >= 0
                ? parsed
                : 0;
        }

        function set(elementId, value) {
            var element =
                document.getElementById(elementId);

            if (element) {
                element.textContent = value;
            }
        }

        function showError(message) {
            success.textContent = "";
            success.style.display = "none";

            error.textContent = message;
            error.style.display = "block";
        }

        function showSuccess(message) {
            error.textContent = "";
            error.style.display = "none";

            success.textContent = message;
            success.style.display = "block";
        }

        function hideMessages() {
            error.textContent = "";
            error.style.display = "none";

            success.textContent = "";
            success.style.display = "none";
        }

        function clearSavedMessageOnEdit() {
            success.textContent = "";
            success.style.display = "none";
        }

        actual.addEventListener(
            "input",
            clearSavedMessageOnEdit
        );

        scrap.addEventListener(
            "input",
            clearSavedMessageOnEdit
        );

        comment.addEventListener(
            "input",
            clearSavedMessageOnEdit
        );

        modal.addEventListener("cancel", function (event) {
            event.preventDefault();
            closeHourModal();
        });

        modal.addEventListener("keydown", function (event) {
            if (event.altKey && event.key === "ArrowLeft") {
                event.preventDefault();
                navigateHour(-1);
            }

            if (event.altKey && event.key === "ArrowRight") {
                event.preventDefault();
                navigateHour(1);
            }
        });

        recalculateCumulativeValues();
        applyLanguage();
        renderProductionTimeline(
            window.productionBoardHours || readTimelineHoursFromBoard()
        );
        applyHourPerformanceColors();
    })();
</script>

<script>
    (function () {
        "use strict";

        var plan =
            window.productionBoardProductPlan ||
            {
                HeaderProductText: "",
                IsMixed: false,
                Products: [],
                Hours: []
            };

        var dialog =
            document.getElementById(
                "productChangeDialog");

        var openButton =
            document.getElementById(
                "changeProductToolbarButton");

        var productSelect =
            document.getElementById(
                "newProductSelect");

        var hourSelect =
            document.getElementById(
                "effectiveHourSelect");

        var changeoverInput =
            document.getElementById(
                "changeoverMinutesInput");

        var rateDisplay =
            document.getElementById(
                "newProductRateDisplay");

        var reasonInput =
            document.getElementById(
                "productChangeReason");

        var preview =
            document.getElementById(
                "productTargetPreview");

        var currentProduct =
            document.getElementById(
                "currentProductAtHour");

        var currentRate =
            document.getElementById(
                "currentRateAtHour");

        var plannedStop =
            document.getElementById(
                "plannedStopAtHour");

        var error =
            document.getElementById(
                "productChangeError");

        var success =
            document.getElementById(
                "productChangeSuccess");

        var confirmButton =
            document.getElementById(
                "confirmProductChangeButton");

        function canEditBoard() {
            var field =
                document.getElementById(
                    "CanEditProductionBoardHiddenField");

            return field &&
                String(field.value)
                    .toLowerCase() === "true";
        }

        function findHour(hourNumber) {
            var hours =
                plan.Hours || [];

            for (var index = 0;
                index < hours.length;
                index++) {
                if (Number(hours[index].HourNumber) ===
                    Number(hourNumber)) {
                    return hours[index];
                }
            }

            return null;
        }

        function findProduct(productId) {
            var products =
                plan.Products || [];

            for (var index = 0;
                index < products.length;
                index++) {
                if (Number(products[index].Id) ===
                    Number(productId)) {
                    return products[index];
                }
            }

            return null;
        }

        function addProductOption(product) {
            var option =
                document.createElement(
                    "option");

            option.value =
                String(product.Id);

            option.textContent =
                product.Name +
                " \u00B7 " +
                product.StandardRatePerHour +
                "/h \u00B7 " +
                formatUnitsPerMinute(
                    product.StandardRatePerHour);

            productSelect.appendChild(
                option);
        }

        function formatUnitsPerMinute(ratePerHour) {
            var value =
                Number(ratePerHour || 0) / 60;

            return value % 1 === 0
                ? value.toFixed(0) + "/min"
                : value.toFixed(2) + "/min";
        }

        function populateProducts() {
            productSelect.innerHTML =
                "";

            var products =
                plan.Products || [];

            for (var index = 0;
                index < products.length;
                index++) {
                addProductOption(
                    products[index]);
            }
        }

        function selectFirstDifferentProduct(
            currentProductId) {
            var products =
                plan.Products || [];

            for (var index = 0;
                index < products.length;
                index++) {
                if (Number(products[index].Id) !==
                    Number(currentProductId)) {
                    productSelect.value =
                        String(products[index].Id);

                    return;
                }
            }
        }

        function getSuggestedHour() {
            var productionHours =
                window.productionBoardHours || [];

            for (var index = 0;
                index < productionHours.length;
                index++) {
                if (Number(
                    productionHours[index]
                        .actualQuantity || 0) === 0) {
                    return Number(
                        productionHours[index]
                            .hourNumber || 1);
                }
            }

            return 8;
        }

        function renderProductPlan() {
            var hours =
                plan.Hours || [];

            for (var index = 0;
                index < hours.length;
                index++) {
                var hour =
                    hours[index];

                var row =
                    document.getElementById(
                        "hourRow" +
                        hour.HourNumber);

                if (!row) {
                    continue;
                }

                var hourCell =
                    row.querySelector(
                        ".hour");

                if (hourCell) {
                    var badge =
                        document.createElement(
                            "span");

                    badge.className =
                        "hour-product-badge";

                    badge.textContent =
                        hour.ProductCode +
                        " \u00B7 " +
                        hour.RatePerHour +
                        "/h";

                    badge.title =
                        hour.ProductName +
                        " \u00B7 " +
                        formatUnitsPerMinute(
                            hour.RatePerHour);

                    hourCell.appendChild(
                        badge);
                }

                if (hour.IsProductChange) {
                    var commentCell =
                        row.querySelector(
                            ".comment");

                    if (commentCell) {
                        var note =
                            document.createElement(
                                "div");

                        note.className =
                            "product-change-note";

                        var icon =
                            document.createElement(
                                "i");

                        icon.className =
                            "fa-solid fa-arrows-rotate";

                        var text =
                            document.createElement(
                                "span");

                        text.textContent =
                            (hour.PreviousProductName ||
                                "Previous product") +
                            " \u2192 " +
                            hour.ProductName +
                            (
                                Number(
                                    hour.ChangeoverMinutes) > 0
                                    ? " \u00B7 " +
                                    hour.ChangeoverMinutes +
                                    " min"
                                    : ""
                            );

                        note.appendChild(
                            icon);

                        note.appendChild(
                            text);

                        commentCell.insertBefore(
                            note,
                            commentCell.firstChild);
                    }
                }
            }
        }

        function updateProductPreview() {
            var hour =
                findHour(
                    Number(hourSelect.value));

            var product =
                findProduct(
                    Number(productSelect.value));

            if (hour) {
                currentProduct.textContent =
                    hour.ProductName;

                currentRate.textContent =
                    hour.RatePerHour +
                    "/h \u00B7 " +
                    formatUnitsPerMinute(
                        hour.RatePerHour);

                plannedStop.textContent =
                    hour.PlannedStopMinutes +
                    " min";
            }

            if (!product || !hour) {
                rateDisplay.value =
                    "";

                preview.textContent =
                    "Select a valid product and hour.";

                return;
            }

            var changeoverMinutes =
                parseNonNegativeInteger(
                    changeoverInput.value);

            if (changeoverMinutes === null) {
                preview.textContent =
                    "Changeover duration must be a non-negative whole number.";

                return;
            }

            var maximumChangeover =
                60 -
                Number(
                    hour.PlannedStopMinutes || 0);

            changeoverInput.max =
                String(maximumChangeover);

            var availableMinutes =
                Math.max(
                    0,
                    60 -
                    Number(
                        hour.PlannedStopMinutes || 0) -
                    changeoverMinutes);

            var target =
                Math.round(
                    Number(
                        product.StandardRatePerHour || 0) *
                    availableMinutes /
                    60);

            rateDisplay.value =
                product.StandardRatePerHour +
                " units/hour \u00B7 " +
                formatUnitsPerMinute(
                    product.StandardRatePerHour);

            preview.innerHTML =
                "<strong>Preview for H" +
                hour.HourNumber +
                ":</strong> " +
                availableMinutes +
                " productive minutes \u00D7 " +
                formatUnitsPerMinute(
                    product.StandardRatePerHour) +
                " = <strong>" +
                target +
                " units</strong>.";
        }

        function parseNonNegativeInteger(value) {
            if (value === null ||
                value === undefined ||
                String(value).trim() === "") {
                return 0;
            }

            var parsed =
                Number(value);

            return Number.isFinite(parsed) &&
                parsed >= 0 &&
                Math.floor(parsed) === parsed
                ? parsed
                : null;
        }

        function showError(message) {
            success.style.display =
                "none";

            success.textContent =
                "";

            error.textContent =
                message;

            error.style.display =
                "block";
        }

        function hideMessages() {
            error.style.display =
                "none";

            error.textContent =
                "";

            success.style.display =
                "none";

            success.textContent =
                "";
        }

        window.openProductChangeModal =
            function () {
                if (!canEditBoard()) {
                    return;
                }

                hideMessages();
                populateProducts();

                var suggestedHour =
                    getSuggestedHour();

                hourSelect.value =
                    String(suggestedHour);

                changeoverInput.value =
                    "0";

                reasonInput.value =
                    "";

                var hour =
                    findHour(
                        suggestedHour);

                if (hour) {
                    selectFirstDifferentProduct(
                        hour.ProductId);
                }

                updateProductPreview();

                if (typeof dialog.showModal ===
                    "function") {
                    dialog.showModal();
                } else {
                    dialog.setAttribute(
                        "open",
                        "open");
                }
            };

        window.closeProductChangeModal =
            function () {
                if (dialog.open) {
                    dialog.close();
                }
            };

        window.submitProductChange =
            function () {
                hideMessages();

                var boardIdField =
                    document.getElementById(
                        "CurrentBoardIdHiddenField");

                var boardId =
                    boardIdField
                        ? Number(boardIdField.value)
                        : 0;

                var newProductId =
                    Number(productSelect.value);

                var effectiveHourNumber =
                    Number(hourSelect.value);

                var changeoverMinutes =
                    parseNonNegativeInteger(
                        changeoverInput.value);

                if (!boardId ||
                    !newProductId ||
                    effectiveHourNumber < 1 ||
                    effectiveHourNumber > 8) {
                    showError(
                        "Select a valid board, product and effective hour.");

                    return;
                }

                if (changeoverMinutes === null) {
                    showError(
                        "Changeover duration must be a non-negative whole number.");

                    return;
                }

                confirmButton.disabled =
                    true;

                confirmButton.textContent =
                    "Saving...";

                fetch(
                    "PB_page.aspx/ChangeProduct",
                    {
                        method: "POST",
                        credentials: "same-origin",
                        headers: {
                            "Content-Type":
                                "application/json; charset=utf-8"
                        },
                        body: JSON.stringify(
                            {
                                request: {
                                    BoardId:
                                        boardId,

                                    NewProductId:
                                        newProductId,

                                    EffectiveHourNumber:
                                        effectiveHourNumber,

                                    ChangeoverMinutes:
                                        changeoverMinutes,

                                    Reason:
                                        reasonInput.value || ""
                                }
                            })
                    }
                )
                    .then(function (response) {
                        if (!response.ok) {
                            throw new Error(
                                "The server returned HTTP " +
                                response.status +
                                ".");
                        }

                        return response.json();
                    })
                    .then(function (payload) {
                        var result =
                            payload &&
                                payload.d !== undefined
                                ? payload.d
                                : payload;

                        if (!result ||
                            result.Success !== true) {
                            throw new Error(
                                result &&
                                    result.Message
                                    ? result.Message
                                    : "The product could not be changed.");
                        }

                        success.textContent =
                            result.Message ||
                            "Product changed successfully.";

                        success.style.display =
                            "block";

                        window.setTimeout(
                            function () {
                                window.location.reload();
                            },
                            450
                        );
                    })
                    .catch(function (exception) {
                        showError(
                            exception &&
                                exception.message
                                ? exception.message
                                : "The product could not be changed.");
                    })
                    .finally(function () {
                        confirmButton.disabled =
                            false;

                        confirmButton.textContent =
                            "Confirm product change";
                    });
            };

        hourSelect.addEventListener(
            "change",
            function () {
                var hour =
                    findHour(
                        Number(
                            hourSelect.value));

                if (hour) {
                    selectFirstDifferentProduct(
                        hour.ProductId);
                }

                updateProductPreview();
            }
        );

        productSelect.addEventListener(
            "change",
            updateProductPreview
        );

        changeoverInput.addEventListener(
            "input",
            updateProductPreview
        );

        dialog.addEventListener(
            "cancel",
            function (event) {
                event.preventDefault();
                closeProductChangeModal();
            }
        );

        if (!canEditBoard() &&
            openButton) {
            openButton.style.display =
                "none";
        }

        renderProductPlan();
    })();
</script>

</body></html>
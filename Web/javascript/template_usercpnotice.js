function getTime(postdate) {

    now = new Date();
    postdate_date = new Date(getDateFromFormat(postdate,'yyyy/MM/dd/ hh:mm:ss'));
    //expiration_date = new Date(expiration);
    days = (now - postdate_date) / 1000 / 60 / 60 / 24;
    daysRound = Math.floor(days);
    hours = (now - postdate_date) / 1000 / 60 / 60 - (24 * daysRound);
    hoursRound = Math.floor(hours);
    minutes = (now - postdate_date) / 1000 /60 - (24 * 60 * daysRound) - (60 * hoursRound);
    minutesRound = Math.floor(minutes);
    seconds = (now - postdate_date) / 1000 - (24 * 60 * 60 * daysRound) - (60 * 60 * hoursRound) - (60 * minutesRound);
    secondsRound = Math.round(seconds);

    var remain = '';
    if(daysRound > 0) {
        remain = daysRound  + '天';
    }
    
    if(hoursRound > 0) {
        remain += hoursRound + '小时';
    }
    
    if(minutesRound > 0) {
        remain += minutesRound + '分';
    }
    
    if(secondsRound > 0) {
        remain += secondsRound + '秒';
    }
   
    window.document.write(remain + '前');
}

function getDateFromFormat(dateString,formatString){
   var regDate = /\d+/g;
   var regFormat = /[YyMmdHhSs]+/g;
   var dateMatches = dateString.match(regDate);
   var formatmatches = formatString.match(regFormat);
   var date = new Date();
    for(var i=0;i<dateMatches.length;i++){
        switch(formatmatches[i].substring(0,1)){
            case 'Y':
            case 'y':
                 date.setFullYear(parseInt(dateMatches[i]));break;
            case 'M':
                 date.setMonth(parseInt(dateMatches[i])-1);break;
            case 'd':
                 date.setDate(parseInt(dateMatches[i]));break;
            case 'H':
            case 'h':
                 date.setHours(parseInt(dateMatches[i]));break;
            case 'm':
                 date.setMinutes(parseInt(dateMatches[i]));break;
            case 's':
                 date.setSeconds(parseInt(dateMatches[i]));break;
         }
     }
    return date;
}

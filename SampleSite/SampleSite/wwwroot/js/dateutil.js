/// dateutil.js

/// <summary>
/// 文字列データ日付変換
/// </summary>
/// <remarks>
/// Varchar(8)の日付文字列データを日付形式に変換
/// </remarks>
/// <typeparam name="strYYYYMMDD">Varchar(8)の日付</typeparam>
/// <returns>yyyy/MM/dd</returns>
function YYYYMMDDToDate(strYYYYMMDD) {
    try {
        //空白なら空白を返す
        if (!strYYYYMMDD || !(strYYYYMMDD.trim())) {
            return '';
        }
        return strYYYYMMDD.substring(0, 4) + "/" + strYYYYMMDD.substring(4, 6) + "/" + strYYYYMMDD.substring(6, 8);
    }
    catch (e) {
        console.error(e.toString());
        return '';
    }
}

/// <summary>
/// 日付データ文字列変換
/// </summary>
/// <remarks>
/// 日付データをVarchar(8)の日付文字列に変換
/// </remarks>
/// <typeparam name="strDate">yyyy/MM/dd</typeparam>
/// <returns>Varchar(8)の日付</returns>
function DateToYYYYMMDD(strDate) {
    try {
        //空白なら空白を返す
        if (!strDate) {
            return '';
        }
        var dt = new Date(strDate);
        return dt.getFullYear() + ('0' + (dt.getMonth() + 1)).slice(-2) + ('0' + dt.getDate()).slice(-2);
    }
    catch (e) {
        console.error(e.toString());
        return '';
    }
}

/// <summary>
/// 文字列データ時刻変換
/// </summary>
/// <remarks>
/// Varchar(9)の時刻文字列データを時刻形式に変換
/// </remarks>
/// <typeparam name="strHHMMSSFFF">Varchar(9)の時刻</typeparam>
/// <returns>HH:mm:ss.fff</returns>
function HHMMSSFFFToTime(strHHMMSSFFF) {
    try {
        //空白なら空白を返す
        if (!strHHMMSSFFF || !(strHHMMSSFFF.trim())) {
            return '';
        }
        return strHHMMSSFFF.substring(0, 2) + ":" + strHHMMSSFFF.substring(2, 4) + ":" + strHHMMSSFFF.substring(4, 6) + "." + strHHMMSSFFF.substring(6, 9);
    }
    catch (e) {
        console.error(e.toString());
        return '';
    }
}

/// <summary>
/// 時刻データ文字列変換
/// </summary>
/// <remarks>
/// 時刻データをVarchar(9)の時刻文字列に変換
/// </remarks>
/// <typeparam name="strTime">HH:mm:ss.fff</typeparam>
/// <returns>Varchar(9)の時刻</returns>
function TimeToHHMMSSFFF(strTime) {
    try {
        //空白なら空白を返す
        if (!strTime) {
            return '';
        }
        // 日付をつけないと変換できない
        var dt = new Date("1900/01/01 " + strTime);
        return ('0' + dt.getHours()).slice(-2) + ('0' + dt.getMinutes()).slice(-2) + ('0' + dt.getSeconds()).slice(-2) + ('00' + dt.getMilliseconds()).slice(-3);
    }
    catch (e) {
        console.error(e.toString());
        return '';
    }
}

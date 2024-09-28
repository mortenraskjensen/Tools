var QueryString = function () {
  // This function is anonymous, is executed immediately and
  // the return value is assigned to QueryString!
  var query_string = {};
  var query = window.location.search.substring(1);
  var vars = query.split("&");
  for (var i=0;i<vars.length;i++) {
    var pair = vars[i].split("=");
        // If first entry with this name
    if (typeof query_string[pair[0]] === "undefined") {
      query_string[pair[0]] = pair[1];
        // If second entry with this name
    } else if (typeof query_string[pair[0]] === "string") {
      var arr = [ query_string[pair[0]], pair[1] ];
      query_string[pair[0]] = arr;
        // If third or later entry with this name
    } else {
      query_string[pair[0]].push(pair[1]);
    }
  }
    return query_string;
} ();

	function GetQS() {
  // This function is anonymous, is executed immediately and
  // the return value is assigned to QueryString!
  var query_string = {};
  var query = window.location.search.substring(1);
  var vars = query.split("&");
  for (var i=0;i<vars.length;i++) {
    var pair = vars[i].split("=");
        // If first entry with this name
    if (typeof query_string[pair[0]] === "undefined") {
      query_string[pair[0]] = pair[1];
        // If second entry with this name
    } else if (typeof query_string[pair[0]] === "string") {
      var arr = [ query_string[pair[0]], pair[1] ];
      query_string[pair[0]] = arr;
        // If third or later entry with this name
    } else {
      query_string[pair[0]].push(pair[1]);
    }
  }
    return query_string;
}
function SetImg(id, name, src )
{
	o = document.getElementById(id);
	//o.onclick =  "javascript:viewPic(this)";
	o.width = 60;
	o.title = name;
	o.src = src;
	o.border = 1;
	//if (id == 1 || id == 16)
	if ( ot !== undefined && ot !== null )
	{
	ot = document.getElementById(id+1000);
	ot.innerHTML = "<font style=\"color: #ffffff;font-size:8px;\">" + name.substring(0,15) + "</font><br/>";
	}
}

function GetObjectValue(o, nullValue)
{
    if ( o !== undefined && o !== null )
	{
		return o;
	}
	return nullValue;
}

var iHasNotPictureTitle = 0; 


function SetImg2(id, od )
{
	o = document.getElementById(id);
    if ( o !== undefined && o !== null )
	{
	  //o.onclick =  "javascript:viewPic(this)";
	  o.width = 60;
      if ( od !== undefined && od !== null )
	  {
		o.title = GetObjectValue(od.title, "");
		o.src = GetObjectValue(od.src, "");
		o.name = GetObjectValue(od.name, "");
		o.border = 1;

		if (iHasNotPictureTitle != 1)
		{
			if ( ot !== undefined && ot !== null )
			{
				ot = document.getElementById(id+1000);
				ot.innerHTML = "<font style=\"color: #ffffff;font-size:8px;\">" + o.title.substring(0,15) + "</font><br/>";
			}
		}
	  }
	}
}



//document.getElementById("stort")

	function viewPic(billed) {
	  var source = billed.src.replace("\\Thumbs\\","\\").replace("/Thumbs/","/");
          source = source.replace(".gif.jpg",".gif").replace(".bmp.jpg",".bmp").replace(".png.jpg",".png");
      var tittel = billed.title;
      if ( tittel === undefined || tittel === null || tittel == "")
      {
        document.getElementById("tittel").innerHTML = "";
      	//document.getElementById("tittel").height = 0;
      }
      else{
      	document.getElementById("tittel").innerHTML = "<font Color=\"#ffffff\"  style=\"padding:-5px; color: #ffffff;font-size:40px;line-height:40px;\">" + tittel + "</font><br/>";
      	//document.getElementById("tittel").height = 20;
	  }

	  document.getElementById("stort").src =source;
	  document.getElementById("stort").alt =source;
	  document.getElementById("stort").name =billed.name;
	  //document.getElementById("stort2").src =source;
	  //document.getElementById("stort2").alt =source;
	  //document.getElementById("stort3").src =source;
	  //document.getElementById("stort3").alt =source;
	  //document.getElementById("stort4").src =source;
	  //document.getElementById("stort4").alt =source;
	  document.getElementById("billedtekst").innerHTML =source.substring(21);
	  document.getElementById("info2").innerHTML = getfield(billed.name,2);
	}
	function viewPicX(billed, str1, str2) {
	  var source = billed.src.replace("\\Thumbs\\","\\").replace("/Thumbs/","/");
          source = source.replace(".gif.jpg",".gif").replace(".bmp.jpg",".bmp").replace(".png.jpg",".png");
	  document.getElementById("stort").src =source;
	  document.getElementById("stort").alt =source;
	  //document.getElementById("stort2").src =source;
	  //document.getElementById("stort2").alt =source;
	  //document.getElementById("stort3").src =source;
	  //document.getElementById("stort3").alt =source;
	  //document.getElementById("stort4").src =source;
	  //document.getElementById("stort4").alt =source;
	  document.getElementById("billedtekst").innerHTML =source.substring(21);
	  document.getElementById("info2").innerHTML = "";//getfield(billed.name,2);
	  document.getElementById("info").innerHTML = getpath(source, "XYZ");
	}
	function GetUrl_(str)
	{
	   return 'file://///diskstation/Lager/Backup/pic/Logo154720/' + str + '.jpg'
	}
	function GetUrl()
	{
	   document.getElementById("InputField").value = QueryString.name;
	   return GetUrl_(QueryString.name);
	}
	function GetUrl2()
	{
	   return GetUrl_(document.getElementById("InputField").value);
	}
	function getNextOld(){
	   var s = document.getElementById("InputField").value;
	   var d = parseInt(s.substring(0,2), 10);
	   var m = parseInt(s.substring(2,4), 10);
	   var y = parseInt(s.substring(4,8), 10);
	   if (d>=31 && m==1 || d>=29 && m==2
	    || d>=31 && m==3 || d>=30 && m==4
	    || d>=31 && m==5 || d>=30 && m==6
	    || d>=31 && m==7 || d>=31 && m==8
	    || d>=30 && m==9 || d>=31 && m==10
	    || d>=30 && m==11 || d>=31 && m==12 )
	   {
	       d = 1;
	       if (m == 12)
	       {
	           m = 1;
	           y++;
	       }
	       else
	       {
	           m++;
	       }
	   }else{
	   	   d++;
	   }
	   var s2 = "";
	   s2 = (d+100).toString(10).substring(1) + (m+100).toString(10).substring(1) + y.toString(10);
	   document.getElementById("InputField").value = s2;
	}
	function getNext(){
	   var s = document.getElementById("InputField").value;
	   var d = parseInt(s.substring(0,2), 10);
	   var m = parseInt(s.substring(2,4), 10);
	   var y = parseInt(s.substring(4,8), 10);
	   document.getElementById("InputField").value = getNextDay(d,m,y);
	   viewPicPath(GetUrl2());
	   document.getElementById("info").innerHTML = "Next";//source.substring(21);
	}
	function getPrev(){
	   var s = document.getElementById("InputField").value;
	   var d = parseInt(s.substring(0,2), 10);
	   var m = parseInt(s.substring(2,4), 10);
	   var y = parseInt(s.substring(4,8), 10);
	   document.getElementById("InputField").value = getPrevDay(d,m,y);
	   viewPicPath(GetUrl2());
	   document.getElementById("info").innerHTML = getfield("a,b_b,c,d",2);//source.substring(21);
	}	function maxDay(m){
		switch(m)
		{
		    case 1:return 31;
		    case 2:return 29;
		    case 3:return 31;
		    case 4:return 30;
		    case 5:return 31;
		    case 6:return 30;
		    case 7:return 31;
		    case 8:return 31;
		    case 9:return 30;
		    case 10:return 31;
		    case 11:return 30;
		    case 12:return 31;
		    //case 1,3,5,7,8,10,12:return 31;
		    //case 4,6,9,11:return 30;
		    //case 2:return 29;
		}
		return 31;
	}
	function getNextDay2(d,m,y){
	   if (d>=31 && m==1 || d>=29 && m==2
	    || d>=31 && m==3 || d>=30 && m==4
	    || d>=31 && m==5 || d>=30 && m==6
	    || d>=31 && m==7 || d>=31 && m==8
	    || d>=30 && m==9 || d>=31 && m==10
	    || d>=30 && m==11 || d>=31 && m==12 )
	   {
	       d = 1;
	       if (m == 12)
	       {
	           m = 1;
	           y++;
	       }
	       else
	       {
	           m++;
	       }
	   }else{
	   	   d++;
	   }
	   return getDay(d,m,y);
	}
	function getPrevDay(d,m,y){
	   if ( d>1 )
	   {
	   	   d--;
	   }else{
		   if ( d==1 )
		   {
		   		if ( m==1 )
		   		{
				   m=12;
				   d=31;
				   y--;
				}else{
				   m = m-1;
				   d = maxDay(m);
				}
	       }
	   }
	   return getDay(d,m,y);
	}
	function getNextDay(d,m,y){
	   var md = maxDay(m);
	   if ( d>=md )
	   {
		   d=1;
			if ( m==12 )
			{
			   m=1;
			   y++;
			}else{
			   m++;
			}
	   }
	   else{
			d++;
	   }
	   return getDay(d,m,y);
	}

	function getDay(d,m,y){
	   return ((d+100).toString(10).substring(1) + (m+100).toString(10).substring(1) + y.toString(10));
	}


	function viewPicPath(str) {
	  var source = str;
	  document.getElementById("stort").src =source;
	  document.getElementById("stort").alt =source;
	  //document.getElementById("stort2").src =source;
	  //document.getElementById("stort2").alt =source;
	  //document.getElementById("stort3").src =source;
	  //document.getElementById("stort3").alt =source;
	  //document.getElementById("stort4").src =source;
	  //document.getElementById("stort4").alt =source;
	  document.getElementById("billedtekst").innerHTML =source;
	  
	}
	function viewPic2(billed) {
	  document.getElementById("stort").src =billed.name;//Hvis billed findes i stor version
	}
	function changeSize(n) {
	  document.getElementById("stort").width =document.getElementById("stort").width*n;
	  //document.getElementById("stort2").width =document.getElementById("stort").width;
	  //document.getElementById("stort3").width =document.getElementById("stort").width;
	  //document.getElementById("stort4").width =document.getElementById("stort").width;
	}
//	function change(n,size) {
//document.getElementById("one").style.visibility = "hidden";
//document.getElementById("two").style.visibility = "visible";// (dom||ie)? "visible"	: "show";
		//for(var i=1;i<=11;i++){
		//	document.getElementById("h" + i.toString(10)).width = size;
		//}
//	}
	function change2(knap,open) {
		var close = document.getElementById("ActiveDiv").value;
		document.getElementById(close).style.visibility = "hidden";
		document.getElementById(open).style.visibility = "visible";// (dom||ie)? "visible"	: "show";
		document.getElementById("ActiveDiv").value = open;
	}
	function togglevisibility(id) {
		if (document.getElementById(id).style.visibility = "visible")
			document.getElementById(id).style.visibility = "hidden";
		else
			document.getElementById(id).style.visibility = "visible";
	}
	function change3(knap,open) {
		var close = document.getElementById("ActiveDiv").value;
		var old_knap = document.getElementById("DisabledButton").value;
		document.getElementById(old_knap).disabled = false;
		document.getElementById(close).style.visibility = "hidden";
		document.getElementById(open).style.visibility = "visible";// (dom||ie)? "visible"	: "show";
		document.getElementById("ActiveDiv").value = open;
		document.getElementById(knap).disabled = true;
		document.getElementById("DisabledButton").value = knap;
	}
	function getfield(s,idx) {
	    var arr = s.split(',');
	    if (idx >= 0)
	    {
	    	if (arr.length > idx)
	    		return arr[idx];
		    return arr[arr.length -1];
	    }
	    if (idx < 0)
	    {
	    	if (-arr.length < idx)
	    		return arr[arr.length+idx];
		    return arr[0];
	    }
	}

	function getslashposition(path)
	{//last slash
	    var idx1 = path.lastIndexOf("/");
	    var idx1b = path.lastIndexOf("\\");
	    if (idx1<idx1b)
	    	idx1 = idx1b;
	    return idx1;
	}
	function getpath(path,name) {
	    var idx1 = getslashposition(path);
	    var idx2 = path.lastIndexOf(".");
	    return path.substring(0, idx1 +1) + name + path.substring(idx2 ,path.length);
	}
	function getnamefrompath(path) {
	    var idx1 = getslashposition(path);
	    var idx2 = path.lastIndexOf(".");
	    return path.substring(idx1 +1,idx2);
	}
	function getpath2(path,name) {
	    var idx1 = getslashposition(path);
	    return path.substring(0, idx1 +1) + name;
	}
	function getnamefrompath2(path) {
	    var idx1 = getslashposition(path);
	    return path.substring(idx1 +1,path.length);
	}
	function getnameidx(name,s,offset) {
	    var arr = s.split(',');
	    var idx = -1;
		for(var i=0;i<arr.length;i++){
			//alert(name + ":" + arr[i]);
			if (name == arr[i])
				idx = i;
		}

	    return adjustidx(idx+offset, arr.length);
	}
	function getnameidx3(name, arr) {
		for(var i=0;i<arr.length;i++)
			if (name == arr[i])
				return i;

	    return 0;
	}
	function getfield3(idx, arr) {
   		return arr[adjustidx(idx, arr.length)];
	}
	function signed2numeric(idx, antal) {
	    if (idx < 0)
	    	return idx + antal;
		else
	    	return idx;
	}
	function adjustidx(idx,antal) {
		if (idx<0)
			return 0;
		if (antal <= idx)
			return antal -1;

		return idx;
	}
	function changesourcerelative(offset) {
		    var source = document.getElementById("stort").src;
		    var list = document.getElementById("stort").name;
		    var name = getnamefrompath(source);
		    var arr  = list.split(',');

			var idx = getnameidx3(name, arr);
			var fld = getfield3(idx, arr);
			document.getElementById("stort").src = getpath(source, fld);
			document.getElementById("info").innerHTML = adjustidx( idx+offset, arr.length) + 1;
			document.getElementById("info2").innerHTML = document.getElementById("stort").src;
			document.getElementById("antal").innerHTML = arr.length;
	}
	function changesourcerelative2(offset) {
		    var source = document.getElementById("stort").src;
		    var list = document.getElementById("stort").name;
		    var name = getnamefrompath2(source);
		    var arr  = list.split(',');

			var idx = getnameidx3(name, arr);
			changesourceN(idx + offset, source, arr);
	}
	function changesource(idx) {
	    var source = document.getElementById("stort").src;
	    var list = document.getElementById("stort").name;
	    var arr  = list.split(',');
	    document.getElementById("stort").src = getpath(source, getfield3(idx, arr));
	    document.getElementById("billedtekst").innerHTML = document.getElementById("stort").src.substring(21);
	    document.getElementById("antal").innerHTML = picnr(adjustidx( idx, arr.length) + 1, arr.length);
	    document.getElementById("info").innerHTML =  "";
	    document.getElementById("info2").innerHTML = "";
	}
	function changesource2(idx) {
		//unsigned
	    var source = document.getElementById("stort").src;
	    var list = document.getElementById("stort").name;
	    var arr  = list.split(',');
	    changesourceN(idx, source, arr);
	}
	function changesource3(idx) {
		//signed
	    var source = document.getElementById("stort").src;
	    var list = document.getElementById("stort").name;
	    var arr  = list.split(',');
	    idx = signed2numeric(idx, arr.length);
	    changesourceN(idx, source, arr);
	}
	function changesourceN(idx, source, arr) {
		var fld = getfield3(idx, arr);
	    document.getElementById("stort").src = getpath2(source, fld);
	    document.getElementById("billedtekst").innerHTML = document.getElementById("stort").src.substring(21);
	    document.getElementById("antal").innerHTML = picnr(adjustidx( idx, arr.length) + 1, arr.length);
	    document.getElementById("info").innerHTML =  "";
	    document.getElementById("info2").innerHTML = "";
	}
    function picnr(nr,antal)
    {
    	return nr.toString() + "/" + antal.toString();
    }

var list = "003,008,014,018,024,030,031,041,048,055,063,070";//var idx = [0,19,38,50];
//var lng = [19,19,11,12];
//var col = ["a","a","h","h"];
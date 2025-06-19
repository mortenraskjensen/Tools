console.log("Sanity check");

const apiKey = `e9ddb24aed6d48c4342303aba5269e28`;
const apiUrl = `https://api.themoviedb.org/3/search/movie?api_key=${apiKey}&query=`;
const imgUrl = `http://image.tmdb.org/t/p/w300/`;
const apimrjUrl = `https://mortenr.azurewebsites.net/`;

/*
//Wrong Way
let globalMovieData = [];
$.ajax({
	url: apiUrl + "Interstellar",
	method: 'get',
	success: (movieData) => {
		console.log(movieData);
		globalMovieData = movieData.results;
	}
});
console.log(globalMovieData);
*/
function getMovieData(movieTitle){
	//console.log(movieTitle);
	return new Promise((resolve,reject)=> {
		$.ajax({
			url: apiUrl + movieTitle,
			method: 'get',
			success: (movieData) => {
				//console.log(movieData);
				resolve(movieData.results);
			},
			error: (errorMsg)=>{
				reject(errorMsg);
			}
		});  //return movieTitle;
	});
}
function getPromiseCalcData(a,b){
	return new Promise((resolve,reject)=> {
		$.ajax({
			url: apimrjUrl + `mul?a=` + a.toString(10) + `&b=` + b.toString(10),
			method: 'get',
			success: (apiData) => {
				//console.log(apiData);
				resolve(apiData.results);
			},
			error: (errorMsg)=>{
				reject(errorMsg);
			}
		});//return apiData;
	});
}



document.getElementById('movie-form').addEventListener('submit', (event)=>{
	event.preventDefault();
	//console.log("Form submitted");
	const movieTitle = document.getElementById('movie-title').value;
	const movieData = getMovieData(movieTitle);
	console.log(movieData);
	movieData
	.then((data)=>{console.log(data);
		document.getElementById('movies').innerHTML = '<img src=' + imgUrl + data[0].poster_path + ' />';
	})
	.catch((data)=>{console.log(data);});
});

document.getElementById('calc-form').addEventListener('submit', (event)=>{
	event.preventDefault();
	//console.log("Form submitted");
	const operanda = document.getElementById('operand-a').value;
	const operandb = document.getElementById('operand-b').value;
	const promiseCalcData = getPromiseCalcData(operanda, operandb);
	console.log(promiseCalcData);
	promiseCalcData
	.then((data)=>{console.log(data);
		document.getElementById('operand-c').innerHTML = data[0].value.toString(10);
	})
	.catch((data)=>{console.log(data);});
});

<?php
/*

forecast:@105976783
            low  hi   ?  wd ?  ?  ws  icon pre% ?   ? ? hum%
#2015-02-21*36.6*46.8*0*NE *0*2.8*2.2*d200*4   *0.0*0*0*61
10x
@

uvi:@105976783
#2015-02-20*1.4
10x
@@

sun:@105976783
#2015-02-21*07:04*17:35
10x
@\n
nextupdate:

*/
$api_key = '';

include_once("cache.php");

function winddeg2letter($deg)
{
	if ($deg >= 349 && $deg < 12)
		return "N";
	else if ($deg >= 12 && $deg < 34)
		return "NNE";
	else if ($deg >= 34 && $deg < 57)
		return "NE";
	else if ($deg >= 57 && $deg < 79)
		return "ENE";
	else if ($deg >= 79 && $deg < 102)
		return "E";
	else if ($deg >= 102 && $deg < 124)
		return "ESE";
	else if ($deg >= 124 && $deg < 147)
		return "SE";
	else if ($deg >= 147 && $deg < 169)
		return "SSE";
	else if ($deg >= 169 && $deg < 192)
		return "S";
	else if ($deg >= 192 && $deg < 214)
		return "SSW";
	else if ($deg >= 214 && $deg < 237)
		return "SW";
	else if ($deg >= 237 && $deg < 256)
		return "WSW";
	else if ($deg >= 256 && $deg < 282)
		return "W";
	else if ($deg >= 282 && $deg < 304)
		return "WNW";
	else if ($deg >= 304 && $deg < 327)
		return "NW";
	else if ($deg >= 327 && $deg < 349)
		return "NNW";
	else
		return "";
}

function temp($tmp)
{
	return $tmp*9/5+32;
}

function weather_code($cod)
{
	$codes = array(
	"d300" => array(200, 210, 230),
	"d342" => array(201, 211, 221, 231),
	"d210" => array(300, 310, 500, 510),
	"d440" => array(202, 212, 232),
	"d230" => array(301, 311, 321, 501, 511, 521, 531),
	"d330" => array(502, 522, 302, 312, 313, 314),
	"d420" => array(600),
	"d432" => array(601, 602),
	"d412" => array(611),
	"d421" => array(612),
	"d232" => array(615),
	"d422" => array(616),
	"d100" => array(800),
	"d300" => array(801, 802, 803),
	"d410" => array(804),
	"d442" => array(900, 901, 902, 903, 904, 905, 906),
	);

	foreach ($codes as $key=>$ary) {
		if (array_key_exists($cod, $ary))
			return $key;
	}

	return "d000";
}

function get_key_exists($ary, $what, $default)
{
	if (array_key_exists($what, $ary)) {
		return $ary[$what];
	} else {
		return $default;
	}
}

function forcacity2owm($city)
{
	return $city - 100000000;
}

$city_forca = get_key_exists($_GET, 'city', 0);
$city = forcacity2owm($city_forca);
if ($city_forca == 0 || $city == 0)
	die();


$cache = new Cache('weather.cache');

$input = $cache->get_key($city);
if ($input == null) {
	$url = 'http://api.openweathermap.org/data/2.5/forecast/daily?units=metric&cnt=10&mode=json&id=' . $city;
	if ($api_key != '') {
		$url .= '&APPID=' . $api_key;
	}
	$input = file_get_contents($url);
	$cache->set_key($city, $input);
}
 
//http://api.openweathermap.org/data/2.5/forecast/daily?id=724503&cnt=10&mode=json

/*$input = '{"cod":"200","message":0.0162,"city":{"id":724503,"name":"Kezmarok","coord":{"lon":20.433519,"lat":49.1357},"country":"SK","population":21391},"cnt":10,"list":[{"dt":1428746400,"temp":{"day":14.39,"min":3.28,"max":15.27,"night":3.28,"eve":12.99,"morn":8.08},"pressure":924.18,"humidity":82,"weather":[{"id":800,"main":"Clear","description":"sky is clear","icon":"01d"}],"speed":1.17,"deg":174,"clouds":0},{"dt":1428832800,"temp":{"day":6.36,"min":-0.58,"max":7.79,"night":-0.58,"eve":7.79,"morn":2.7},"pressure":928.71,"humidity":99,"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10d"}],"speed":1.24,"deg":316,"clouds":68,"rain":2.55},{"dt":1428919200,"temp":{"day":9.54,"min":-1.83,"max":11.1,"night":4.96,"eve":10.77,"morn":-1.83},"pressure":925.64,"humidity":87,"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10d"}],"speed":2.76,"deg":250,"clouds":24,"rain":1.53},{"dt":1429005600,"temp":{"day":5.94,"min":0.27,"max":5.94,"night":0.27,"eve":2.69,"morn":2.18},"pressure":960.88,"humidity":0,"weather":[{"id":600,"main":"Snow","description":"light snow","icon":"13d"}],"speed":2.12,"deg":305,"clouds":30,"rain":0.77,"snow":0.54},{"dt":1429092000,"temp":{"day":8.33,"min":1.14,"max":8.33,"night":1.14,"eve":6.92,"morn":2.68},"pressure":957.14,"humidity":0,"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10d"}],"speed":1.71,"deg":266,"clouds":71,"rain":1.68},{"dt":1429178400,"temp":{"day":14.42,"min":4.33,"max":14.42,"night":4.33,"eve":9.98,"morn":5.97},"pressure":957.75,"humidity":0,"weather":[{"id":800,"main":"Clear","description":"sky is clear","icon":"01d"}],"speed":2.14,"deg":278,"clouds":4},{"dt":1429264800,"temp":{"day":13.79,"min":8.84,"max":13.79,"night":10.23,"eve":12.02,"morn":8.84},"pressure":951.77,"humidity":0,"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10d"}],"speed":3.34,"deg":271,"clouds":85,"rain":2.15},{"dt":1429351200,"temp":{"day":14.96,"min":8.15,"max":14.96,"night":8.15,"eve":11.64,"morn":9.65},"pressure":949.28,"humidity":0,"weather":[{"id":500,"main":"Rain","description":"light rain","icon":"10d"}],"speed":1.43,"deg":271,"clouds":86,"rain":1.31},{"dt":1429437600,"temp":{"day":14.96,"min":8.67,"max":14.96,"night":8.67,"eve":12.3,"morn":11.2},"pressure":942.32,"humidity":0,"weather":[{"id":501,"main":"Rain","description":"moderate rain","icon":"10d"}],"speed":2.13,"deg":246,"clouds":83,"rain":6.61},{"dt":1429524000,"temp":{"day":11.66,"min":6.02,"max":11.66,"night":6.02,"eve":8.41,"morn":8.68},"pressure":932.73,"humidity":0,"weather":[{"id":501,"main":"Rain","description":"moderate rain","icon":"10d"}],"speed":0.81,"deg":343,"clouds":53,"rain":7.45}]}';
*/
$weather = json_decode($input, true);

//var_dump($weather);

$data = "forecast:@105976783";

for ($i = 0; $i < 10; $i++) {
	$day = $weather['list'][$i];

	$data .= '#' . date('Y-m-d', strtotime("+".$i." day"));
	$data .= '*' . temp($day['temp']['min']);
	$data .= '*' . temp($day['temp']['max']);
	$data .= '*' . '0';
	$data .= '*' . winddeg2letter($day['deg']);
	$data .= '*' . '0';
	$data .= '*' . '0';
	$data .= '*' . $day['speed'];
	$data .= '*' . weather_code($day['weather'][0]['id']); //'d200';
	$data .= '*' . '0'; // percipitation %
	$data .= '*' . '0';
	$data .= '*' . '0';
	$data .= '*' . '0';
	$data .= '*' . $day['humidity'];
}
$data .= '@';

$data .= 'uvi:@105976783';
for ($i = 0; $i < 10; $i++) {
	$data .= '#' . date('Y-m-d', strtotime("+".$i." day"));
	$data .= '*0';
}
$data .= '@@';

$data .= 'sun:@105976783';
for ($i = 0; $i < 10; $i++) {
	$data .= '#' . date('Y-m-d', strtotime("+".$i." day"));
	$data .= '*06:00'; // sunset
	$data .= '*18:00'; // sunrise
}
$data .= '@'."\nnextupdate:000";

//$data = "forecast:@105976783#2015-04-11*36.6*46.8*0*NE*0*2.8*2.2*d200*4*0.0*0*0*61#2015-02-22*32.7*50.3*0*E*0*2.0*1.3*d000*1*0*0*0*63#2015-04-12*40.5*53.9*0*SE*0*1.4*0.9*d100*1*0*0*0*61#2015-04-13*38.0*46.1*0*WSW*0*2.6*1.3*d200*7*0.3*0*0*77#2015-04-14*37.4*43.6*0*WSW*0*1.8*1.1*d420*98*2.6*0*0*91#2015-04-15*32.8*45.2*0*NNE*0*2.5*1.7*d100*13*0.0*0*0*65#2015-05-16*31.2*41.2*0*NNE*0*2.8*1.9*d200*23*0.0*0*0*54#2015-04-17*27.8*44.8*0*ENE*0*1.6*0.9*d000*3*0.0*0*0*54#2015-04-18*30.5*45.2*0*ENE*0*2.0*1.3*d200*10*0.0*0*0*61#2015-04-19*32.7*47.5*0*NNE*0*2.5*1.6*d200*18*0.1*0*0*64@uvi:@105976783#2015-04-11*1.4#2015-02-21*1.6#2015-04-12*2.0#2015-02-23*2.1#2015-04-13*1.8@sun:@105976783#2015-04-11*07:04*17:35#2015-04-12*07:02*17:37#2015-04-13*07:00*17:38#2015-04-14*06:58*17:40#2015-04-15*06:56*17:42#2015-04-16*06:54*17:43#2015-04-17*06:52*17:45#2015-04-18*06:50*17:46#2015-04-19*06:48*17:48#2015-04-20*06:46*17:50#2015-04-21*06:44*17:51#2015-04-22*06:42*ÿ,TJTvX]";

$key = "ansen's weather";
echo openssl_encrypt($data, "des-ede3", substr(md5($key), 0, 24), $options=0, "");

?>
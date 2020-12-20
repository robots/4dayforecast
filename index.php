<?php

$api_key = '';

include_once("cache.php");

function temp($tmp)
{
	return $tmp*9/5+32;
}

function weather_code($cod)
{
	$codes = array(
	200 => "d240",
	201 => "d340",
	202 => "d440",
	230 => "d240",
	231 => "d340",
	232 => "d340",
	233 => "d440",
	300 => "d310",
	301 => "d310",
	302 => "d310",
	500 => "d410",
	501 => "d420",
	502 => "d430",
	511 => "d411", 
	520 => "d410",
	521 => "d220",
	522 => "d312",
	600 => "d312",
	601 => "d422",
	602 => "d432",
	610 => "d321",
	611 => "d311",
	612 => "d431",
	621 => "d222",
	622 => "d322",
	623 => "d412",
	700 => "d210",
	711 => "d500",
	721 => "d500",
	731 => "d310",
	741 => "d300",
	751 => "d211",
	800 => "d000",
	801 => "d200",
	802 => "d200",
	803 => "d300",
	804 => "d400",
	900 => "d430"
	);

	return $codes[$cod];
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
	$url = 'http://api.weatherbit.io/v2.0/forecast/daily?city_id=' . $city;
	if ($api_key != '') {
		$url .= '&key=' . $api_key;
	}
	$url .= '&days=10';
	$input = file_get_contents($url);
	$cache->set_key($city, $input);
}
 
$weather = json_decode($input, true);

$timezone = new DateTimeZone($weather['timezone']);
$offset = $timezone->getOffset(new DateTime);

$data = "forecast:@". $city_forca;

for ($i = 0; $i < 10; $i++) {
	$day = $weather['data'][$i];
	$data .= '#' . $day['datetime'];
	$data .= '*' . temp($day['min_temp']);
	$data .= '*' . temp($day['max_temp']);
	$data .= '*' . '0';
	$data .= '*' . $day['wind_cdir'];
	$data .= '*' . '0';
	$data .= '*' . '0';
	$data .= '*' . $day['wind_spd'];
	$data .= '*' . weather_code($day['weather']['code']); //'d200';
	$data .= '*' . $day['pop']; // percipitation %
	$data .= '*' . $day['precip'];
	$data .= '*' . '0';
	$data .= '*' . '0';
	$data .= '*' . $day['rh'];
}

$data .= '@';

$data .= 'uvi:@'. $city_forca;
for ($i = 0; $i < 10; $i++) {
	$day = $weather['data'][$i];
	$data .= '#' . $day['datetime'];
	$data .= '*'. $day['uv'];
}
$data .= '@@';

$url = 'http://api.weatherbit.io/v2.0/current?city_id='. $city. '&key='. $api_key;
$input = file_get_contents($url);
$weather = json_decode($input, true);

$sunrise = date("H:i", strtotime($weather['data'][0]['sunrise'])+$offset);
$sunset = date("H:i", strtotime($weather['data'][0]['sunset'])+$offset);

$data .= 'sun:@'. $city_forca;
for ($i = 0; $i < 10; $i++) {
	$data .= '#' . date('Y-m-d', $weather['data'][0]['ts']+$offset+24*3600*$i);
	$data .= '*'. $sunrise;
	$data .= '*'. $sunset;
}
$data .= '@'."\nnextupdate:000";
$key = "ansen's weather";
echo openssl_encrypt($data, "des-ede3", substr(md5($key), 0, 24), $options=0, "");

?>
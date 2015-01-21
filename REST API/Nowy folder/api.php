<?php
//Dane dostêpu do bazy danych
$pool_conn['my_host'] = "localhost";
$pool_conn['db_name'] = "rpi_alarm";
//Tabela pobierana jest z argumentu podawanego w linku
$pool_conn['table_name'] = htmlspecialchars($_GET["table"]);
$pool_conn['my_user'] = "exten";
$pool_conn['my_pass'] = "mp12345";
//Pobieranie opcjonalnych argumentów przedzia³u czasu
if($_GET["min"] === null)
	$pool_conn['min_date'] = "(select min(DATE) from ".$pool_conn['table_name'].")";
else
	$pool_conn['min_date'] = "'".htmlspecialchars($_GET["min"])."'";
if($_GET["max"] === null)
	$pool_conn['max_date'] = "(select max(DATE) from ".$pool_conn['table_name'].")";
else
	$pool_conn['max_date'] = "'".htmlspecialchars($_GET["max"])."'";

//Po³¹czenie z baz¹ danych
mysql_connect($pool_conn['my_host'],$pool_conn['my_user'],$pool_conn['my_pass']);
@mysql_select_db($pool_conn['db_name']) or die( "Unable to select database");
//Utworzenie dokumentu xml
$xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
$root_element .= $pool_conn['table_name']."s";
$xml .= "<$root_element>";

//Pobranie danych z danej tabeli
$sql = "SELECT * FROM ".$pool_conn['table_name']." WHERE DATE >= ".$pool_conn['min_date']." AND DATE <= ".$pool_conn['max_date'];
$result = mysql_query($sql);
if (!$result) {
    die('Invalid query: ' . mysql_error());
}
 
//Sprawdzenie, czy zapytanie zwróci³o wyniki i rozpoczêcie pêtli tworz¹cej dokument xml
if(mysql_num_rows($result)>0)
{
   while($result_array = mysql_fetch_assoc($result))
   {
      $xml .= "<".$pool_conn['table_name'].">";
      foreach($result_array as $key => $value)
      {
         //Znacznik zawieraj¹cy nazwê tabeli
         $xml .= "<$key>";
 
		//Dodanie pomiêdzy znacznikami wartoœci z kolumny
         $xml .= "$value"; 
 
		//Zamkniêcie elemntu
         $xml .= "</$key>";
      } 
      $xml.="</".$pool_conn['table_name'].">";
   }
}

//Zamkniêcie korzenia
$xml .= "</$root_element>";
 
//Wys³anie nag³ówka dokumentu xml do przegl¹darki
header ("Content-Type:text/xml"); 

//Zwrócenie dokumentu
echo $xml;
?>
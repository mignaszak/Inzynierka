<?php
//db access
$pool_conn['my_host'] = "localhost";
$pool_conn['db_name'] = "rpi_alarm";
$pool_conn['table_name'] = htmlspecialchars($_GET["table"]);
$pool_conn['my_user'] = "root";
$pool_conn['my_pass'] = "mikipi123";
if($_GET["min"] === null)
	$pool_conn['min_date'] = "(select min(DATE) from ".$pool_conn['table_name'].")";
else
	$pool_conn['min_date'] = "'".htmlspecialchars($_GET["min"])."'";
if($_GET["max"] === null)
	$pool_conn['max_date'] = "(select max(DATE) from ".$pool_conn['table_name'].")";
else
	$pool_conn['max_date'] = "'".htmlspecialchars($_GET["max"])."'";

//	echo $pool_conn['min_date'];
//	echo $pool_conn['max_date'] ;
	
//db connection 
mysql_connect($pool_conn['my_host'],$pool_conn['my_user'],$pool_conn['my_pass']);

@mysql_select_db($pool_conn['db_name']) or die( "Unable to select database");
//xml create
$xml = "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>";
//$xml .= "<root xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">";
$root_element .= $pool_conn['table_name']."s";
$xml .= "<$root_element>";


//select items in table
//$sql = "SELECT * FROM ".$pool_conn['table_name']." WHERE DATE >= ".$pool_conn['min_date']." AND DATE =< ".$pool_conn['max_date'];
$sql = "SELECT * FROM ".$pool_conn['table_name']." WHERE DATE >= ".$pool_conn['min_date']." AND DATE <= ".$pool_conn['max_date'];

$result = mysql_query($sql);
if (!$result) {
    die('Invalid query: ' . mysql_error());
}
 
if(mysql_num_rows($result)>0)
{
   while($result_array = mysql_fetch_assoc($result))
   {
      $xml .= "<".$pool_conn['table_name'].">";
 
      
      foreach($result_array as $key => $value)
      {
         //$key table column name
         $xml .= "<$key>";
 
         //embed the SQL data
         $xml .= "$value"; 
 
         //close the element
         $xml .= "</$key>";
      }
 
      $xml.="</".$pool_conn['table_name'].">";
   }
}

//close the root element
$xml .= "</$root_element>";
//close root
//$xml .= "</root>";
 
//send the xml header to the browser
header ("Content-Type:text/xml"); 

//output the XML data
echo $xml;

?>
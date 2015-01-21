<?php
if($_GET["min"] === null)
	$pool_conn['min_date'] = "(select min(DATE) from ".$pool_conn['table_name'].")";
else
	if(strlen($_GET["min"])==10)
		$pool_conn['min_date'] = "'".htmlspecialchars($_GET["min"])." 00:00:00'";
	else
		$pool_conn['min_date'] = "'".htmlspecialchars($_GET["min"])."'";
	
	echo $pool_conn['min_date'] 
?>
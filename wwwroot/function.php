<?php
session_start();


date_default_timezone_set('Europe/Moscow');



if ($_SERVER['REQUEST_METHOD'] == 'POST' && isset($_POST['min-price'])) {
    
      

}   

if (isset($_POST['addProvider'])) {

    $nameProvider = !empty($_POST['nameProvider']) ? trim($_POST['nameProvider']) : '';
    $descriptionProvider = !empty($_POST['descriptionProvider']) ? trim($_POST['descriptionProvider']) : '';
    $ratingProvider = !empty($_POST['ratingProvider']) ? trim($_POST['ratingProvider']) : '';
    $descriptionProvider = htmlspecialchars($descriptionProvider);
    $ratingProvider = htmlspecialchars($ratingProvider);
    if (empty($nameProvider) ||  empty($descriptionProvider)||  empty($ratingProvider)|| empty($_FILES["imageProvider"]["tmp_name"])) {
        $_SESSION['adderror'] = "Поля не могут быть пустыми".' '.'('. $new_time = date('H:i:s').' '.'МСК'.' '.')';
    }
    else{
        // Путь к папке, куда будет загружено изображение
        $target_dir = $_SERVER['DOCUMENT_ROOT']."/img/";
        // Имя файла
        $target_file = $target_dir . basename($_FILES["imageProvider"]["name"]);
        // Проверяем, является ли файл изображением
        $check = getimagesize($_FILES["imageProvider"]["tmp_name"]);
        if($check !== false) {
            // Загружаем файл на сервер
            if(move_uploaded_file($_FILES["imageProvider"]["tmp_name"], $target_file)) {
                $iconPath ="/assets/img/".$_FILES["imageProvider"]["name"];
            
                $stmt= $pdo->prepare("INSERT INTO provaider_card (name, icon, description, rating) VALUES (?,?,?,?)");
                $stmt->execute([$nameProvider, $iconPath, $descriptionProvider, $ratingProvider]);
            

                $_SESSION['add'] = "Провайдер добавлен".' '.'('. $new_time = date('H:i:s').' '.'МСК'.' '.')';
            } else {
                $_SESSION['adderror'] = "Ошибка при записи данных".' '.'('. $new_time = date('H:i:s').' '.'МСК'.' '.')';
            }
        } else {
            $_SESSION['adderror'] = "Выбранный файл не являет картинкой".' '.'('. $new_time = date('H:i:s').' '.'МСК'.' '.')';
        }
    }
}


if (isset($_POST['addTarif'])) {

    $login = !empty($_POST['login']) ? trim($_POST['login']) : '';
    $password = !empty($_POST['password']) ? trim($_POST['password']) : '';
    
    $login = htmlspecialchars($login);
    $password = htmlspecialchars($password);

    if (empty($login) ||  empty($password)) {
        $_SESSION['adderror'] = "Поля не могут быть пустыми".' '.'('. $new_time = date('H:i:s').' '.'МСК'.' '.')';
    }
    else{
        $url = 'http://example.com/api/GetTitle';
        $data = array('login' => 'value1', 'password' => 'value2');
        
        $curl = curl_init($url);
        curl_setopt($curl, CURLOPT_POST, true);
        curl_setopt($curl, CURLOPT_POSTFIELDS, $data);
        curl_setopt($curl, CURLOPT_RETURNTRANSFER, true);
        
        $response = curl_exec($curl);
        
        curl_close($curl);

    }

}



if (isset($_POST['deleteProvider'])) {

    if ($_POST['deleteProvider'] == 'deleteProvider') {
        deleteProvider();
    }

}

if (isset($_POST['deleteTarif'])) {

    if ($_POST['deleteTarif'] == 'deleteTarif') {
        deleteTarif();
    }

}

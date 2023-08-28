<?php
error_reporting(0);
?>
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <link rel="stylesheet" href="style/index.css"/>
    <link rel="preconnect" href="https://fonts.googleapis.com">
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
    <link rel="apple-touch-icon" sizes="180x180" href="style/apple-touch-icon.png">
    <link rel="icon" type="image/png" sizes="32x32" href="style/favicon-32x32.png">
    <link rel="icon" type="image/png" sizes="16x16" href="style/favicon-16x16.png">
    <link rel="manifest" href="style/site.webmanifest">
    <link href="https://fonts.googleapis.com/css2?family=Lato&display=swap" rel="stylesheet">
    <title>Liquid store</title>
</head>
<body>
    <div class="title"><h1>CENNIK</h1></div>
    <div class="price_list_div">
        <table>
            <tr><th>ML</th><th>0</th><th>6</th><th>12</th><th>18</ht></tr>
            <tr><th>30</th><td>25</td><td>25</td><td>25</td><td>25</td></tr>
            <tr><th>50</th><td>38</td><td>38</td><td>38</td><td>38</td></tr>
            <tr><th>100</th><td>70</td><td>70</td><td>70</td><td>70</td></tr>
        </table>
    </div>
    <div class="title"><h2>Smaki</h2></div>
    <div class="storage_table">
        <table>
            <tr>
                <th>Marka</th>
                <th>Nazwa</th>
                <th>Zostało na [ml]</th>
            </tr>
            <?php
                $conn = new mysqli("localhost", "root", "", "store");

                if($conn -> connect_error){
                    die("Connection failed! ".$conn->connect_error);
                }
                $sql = "SELECT * FROM STORAGE WHERE REMAINING>0;";
                $result = $conn->query($sql);
                if($result->num_rows > 0){
                    while($row = $result->fetch_assoc()){
                        echo "<tr><td>".$row['Brand']."</td><td>".$row['Name']."</td><td>".($row['Remaining']*10)."</tr>";
                    }
                }

                $conn->close();
            ?>
        </table>
    </div>
</body>
</html>
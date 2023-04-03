using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public class LanguageHandler : MonoBehaviour
{
    // public Configuration configuration;
    public string lang;
    void Awake(){
        // GameObject gameObject = new GameObject("Configuration");
        // configuration = gameObject.AddComponent<Configuration>();
        lang = PlayerPrefs.GetString("Language", "en");
    }
    
    // GameObject gameObject = new GameObject("LanguageHandler");
    // languageHandler = gameObject.AddComponent<LanguageHandler>();
    public Dictionary<string, string> dict;
    
    public void m_dictionary(){
        var languages = new Dictionary<string, Dictionary<string, string>>(){
            {
                "en", new Dictionary<string, string>(){
                    {"ChooseTrack", "Choose a Track"},
                    {"UrbanTrack1", "Urban Track 1"},
                    {"UrbanTrack2", "Urban Track 2"},
                    {"RaceTrack1", "Race Track 1"},
                    {"RaceTrack2", "Race Track 2"},
                    {"CameraCalibration", "Camera Calibration"},
                    {"Exit", "Exit"},
                    {"Status", "Status"},
                    {"SteeringAngle", "Steering Angle"},
                    {"Speed", "Speed (KPH)"},
                    {"Sensors", "Sensors"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Start Server"},
                    {"ServerIP", "Server IP : 127.0.0.1"},
                    {"ServerPort", "Server Port : 25001"},
                    {"LapsAndCheckpointsHeader", "Laps and Checkpoints"},
                    {"LapsAndCheckpoints", "Laps and Checkpoints :\n => Laps : {0} \n => Checkpoints : {1}/{2}"},
                    {"Lap", "Lap {0} : {1}/{2}\n"},
                    {"Configuration", "Configuration"},
                    {"Obstacles", "Obstacles"},
                    {"ManualControl", "Manual Control"},
                    {"RightLaneCheckpoint", "Right Lane Checkpoint"},
                    {"VisibleSensorRay", "Visible Sensor Ray"},
                    {"TopSpeed", "Top Speed"},
                    {"SensorAngle", "Sensor Angle from center :"},
                    {"Degrees", " Degrees"},
                    {"Logs","Logs"},
                    {"StartedServerMsg","\n -> Started at {0} PORT : {1}"},
                    {"Reset","Reset"},
                    {"QuitToMenu","Quit to menu"},
                    {"CloseThisPanel","Close this Panel"},
                    {"InfoPanel","Info Panel"},
                    {"Help","->Help:" 
                        +"\n\tUse Mouse to rotate the view"
                        +"\n\tUse W A S D to move around the scene"
                        +"\n\tUse E and Q to fly up and down in the scene"
                        +"\n\tUse Arrows to move around the scene"},
                    {"Settings", "Settings"},
                    {"RaceFinal", "Race Final"},
                    {"UrbanFinal", "Urban Final"},
                    {"About", "About"},
                    {"aboutThis", "About this simulator"},
                    {"terms","Terms of use"}
                }
            },
            {
                "ru", new Dictionary<string, string>(){
                    {"ChooseTrack", "Выберите трек"},
                    {"UrbanTrack1", "Город (Трек 1)"},
                    {"UrbanTrack2", "Город (Трек 2)"},
                    {"RaceTrack1", "Шоссе (Трек 1)"},
                    {"RaceTrack2", "Шоссе (Трек 2)"},
                    {"CameraCalibration", "Калибровка камеры"},
                    {"Exit", "Выход"},
                    {"Status", "Статус"},
                    {"SteeringAngle", "Угол поворота"},
                    {"Speed", "Скорость (км/ч)"},
                    {"Sensors", "Датчики"},
                    {"cm", " см"},
                    {"KPH", " (км/ч)"},
                    {"StartServer", "Запустить сервер"},
                    {"ServerIP", "IP адрес сервера: 127.0.0.1"},
                    {"ServerPort", "Порт сервера: 25001"},
                    {"LapsAndCheckpointsHeader", "Круги и чекпоинты"},
                    {"LapsAndCheckpoints", "Круги и чекпоинты :\n => Круги : {0} \n => Чекпоинт : {1} из {2}"},
                    {"Lap", " Круг {0} : {1}/{2}\n"},
                    {"Configuration", "Настройки"},
                    {"Obstacles", "Препятствия"},
                    {"ManualControl", "Ручное управление"},
                    {"RightLaneCheckpoint", "Контроль пересечения правой линии"},
                    {"VisibleSensorRay", "Видимость луча датчика"},
                    {"TopSpeed", "Максимальная скорость"},
                    {"SensorAngle", "Угол датчика из центра : "},
                    {"Degrees", " (град.)"},
                    {"Logs", "Логи"},
                    {"StartedServerMsg","\n -> Запущен на IP : {0}, Порт : {1}"},
                    {"Reset", "Сброс"},
                    {"QuitToMenu","Выйти в меню"},
                    {"CloseThisPanel", "Закрыть эту панель"},
                    {"InfoPanel", "Панель информации"},
                    {"Help", "->Помощь:" 
                        +"\n\tИспользуйте мышку для поворота камеры"
                        +"\n\tИспользуйте клавиши W A S D для движения автомобиля"
                        +"\n\tИспользуйте стрелки для движения автомобиля"
                        +"\n\tИспользуйте E и Q для регулировки высоты камеры"},
                    {"Settings", "настройки"},
                    {"RaceFinal", "финал гонки"},
                    {"UrbanFinal", "городской финал"},
                    {"About", "Около"},
                    {"aboutThis", "Об этом симуляторе"},
                    {"terms","Условия эксплуатации"}
                }
            },
            {
                "de", new Dictionary<string, string>(){
                    {"ChooseTrack", "Wählen Sie eine Spur"},
                    {"UrbanTrack1", "Stadtbahn 1"},
                    {"UrbanTrack2", "Stadtbahn 2"},
                    {"RaceTrack1", "Rennstrecke 1"},
                    {"RaceTrack2", "Rennstrecke 2"},
                    {"CameraCalibration", "Kamerakalibrierung"},
                    {"Exit", "Ausgang"},
                    {"Status", "Status"},
                    {"SteeringAngle", "Lenkwinkel"},
                    {"Speed", "Geschwindigkeit (KPH)"},
                    {"Sensors", "Sensoren"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Server Starten"},
                    {"ServerIP", "Server IP : 127.0.0.1"},
                    {"ServerPort", "Server Port : 25001"},
                    {"LapsAndCheckpointsHeader", "Runden und Checkpoints"},
                    {"LapsAndCheckpoints", "Runden und Checkpoints \n => Runden: {0} \n => Checkpoints: {1}/{2}"},
                    {"Lap", "Runde {0}: {1}/{2}\n"},
                    {"Configuration", "Aufbau"},
                    {"Obstacles", "Hindernisse"},
                    {"ManualControl", "Manuelle Kontrolle"},
                    {"RightLaneCheckpoint", "rechter Kontrollpunkt"},
                    {"VisibleSensorRay", "Sichtbarer Sensorstrahl"},
                    {"TopSpeed", "Höchstgeschwindigkeit"},
                    {"SensorAngle", "Sensorwinkel von der Mitte: "},
                    {"Degrees", " Grad"},
                    {"Logs","Protokolle"},
                    {"StartedServerMsg","\n -> Begonnen bei {0} PORT: {1}"},
                    {"Reset","Zurücksetzen"},
                    {"QuitToMenu","Zum Menü zurückkehren"},
                    {"CloseThisPanel","Schließen Sie dieses Fenster"},
                    {"InfoPanel","Infofenster"},
                    {"Help","->Hilf:" 
                        +"\n\tVerwenden Sie die Maus, um die Ansicht zu drehen"
                        +"\n\tVerwenden Sie W A S D, um sich in der Szene zu bewegen"
                        +"\n\tVerwenden Sie E und Q, um in der Szene auf und ab zu fliegen"
                        +"\n\tVerwenden Sie Pfeile, um sich in der Szene zu bewegene:"},
                    {"Settings", "die Einstellungen"},
                    {"RaceFinal", "Rennfinale"},
                    {"UrbanFinal", "städtisches Finale"},
                    {"About", "Über"},
                    {"aboutThis", "Über diesen Simulator"},
                    {"terms","Nutzungsbedingungen"}
                }
            },
            {
                "jp", new Dictionary<string, string>(){
                    {"ChooseTrack", "トラックを選択してください"},
                    {"UrbanTrack1", "アーバントラック (1)"},
                    {"UrbanTrack2", "アーバントラック (2)"},
                    {"RaceTrack1", "レーストラック (1)"},
                    {"RaceTrack2", "レーストラック (2)"},
                    {"CameraCalibration", "カメラのキャリブレーション"},
                    {"Exit", "出口"},
                    {"Status", "状態"},
                    {"SteeringAngle", "操舵角"},
                    {"Speed", "速度（KPH)"},
                    {"Sensors", "センサー"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "サーバーを起動します"},
                    {"ServerIP", "サーバーIP：127.0.0.1"},
                    {"ServerPort", "サーバーポート：25001"},
                    {"LapsAndCheckpointsHeader", "ラップとチェックポイント"},
                    {"LapsAndCheckpoints", "ラップとチェックポイント\n => =>ラップ：{0} \n => チェックポイント：{1}/{2}"},
                    {"Lap", "ラップ{0}：{1}/{2}\n"},
                    {"Configuration", "構成"},
                    {"Obstacles", "障害物"},
                    {"ManualControl", "手動制御"},
                    {"RightLaneCheckpoint", "右車線チェックポイント"},
                    {"VisibleSensorRay", "可視センサー光線"},
                    {"TopSpeed", "最高速度"},
                    {"SensorAngle", "中心からのセンサー角度："},
                    {"Degrees", " 度"},
                    {"Logs","ログ"},
                    {"StartedServerMsg","\n -> {0}ポートで開始：{1}"},
                    {"Reset","リセット"},
                    {"QuitToMenu","メニューを終了します"},
                    {"CloseThisPanel","このパネルを閉じる"},
                    {"InfoPanel","情報パネル"},
                    {"Help","->ヘルプ:" 
                        +"\n\tマウスを使用してビューを回転します"
                        +"\n\tW A S Dを使用してシーン内を移動します"
                        +"\n\tEとQを使用して、シーン内を上下に飛行します"
                        +"\n\t矢印を使用してシーン内を移動します"},
                    {"Settings", "設定"},
                    {"RaceFinal", "決勝レース"},
                    {"UrbanFinal", "アーバンファイナル"},
                    {"About", "約"},
                    {"aboutThis", "このシミュレータについて"},
                    {"terms","利用規約"}
                }
            },
            {
                "kr", new Dictionary<string, string>(){
                    {"ChooseTrack", "트랙 선택"},
                    {"UrbanTrack1", "어반 트랙 (1)"},
                    {"UrbanTrack2", "어반 트랙 (2)"},
                    {"RaceTrack1", "레이스 트랙 (1)"},
                    {"RaceTrack2", "레이스 트랙 (2)"},
                    {"CameraCalibration", "카메라 보정"},
                    {"Exit", "출구"},
                    {"Status", "상태"},
                    {"SteeringAngle", "조향 각도"},
                    {"Speed", "속도 (KPH)"},
                    {"Sensors", "센서"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "서버 시작"},
                    {"ServerIP", "서버 IP : 127.0.0.1"},
                    {"ServerPort", "서버 포트 : 25001"},
                    {"LapsAndCheckpointsHeader", "랩 및 체크 포인트"},
                    {"LapsAndCheckpoints", "랩 및 체크 포인트 \n => 일주: {0} \n => 검문소: {1}/{2}"},
                    {"Lap", "일주 {0}: {1}/{2}\n"},
                    {"Configuration", "구성"},
                    {"Obstacles", "장애물"},
                    {"ManualControl", "수동 제어"},
                    {"RightLaneCheckpoint", "우측 차선 검문소"},
                    {"VisibleSensorRay", "가시 광선"},
                    {"TopSpeed", "최고 속도"},
                    {"SensorAngle", "중심으로부터의 센서 각도 :"},
                    {"Degrees", " 도"},
                    {"Logs","로그"},
                    {"StartedServerMsg","\n ->{0} 포트에서 시작 : {1}"},
                    {"Reset","초기화"},
                    {"QuitToMenu","메뉴 종료"},
                    {"CloseThisPanel","이 패널 닫기"},
                    {"InfoPanel","정보 패널"},
                    {"Help","->도움말:" 
                        +"\n\t마우스를 사용하여보기 회전"
                        +"\n\tW A S D를 사용하여 장면에서 이동"
                        +"\n\tE와 Q를 사용하여 장면에서 위아래로 비행"
                        +"\n\t화살표를 사용하여 장면 주위로 이동"},
                    {"Settings", "설정"},
                    {"RaceFinal", "레이스 결승"},
                    {"UrbanFinal", "어반 파이널"},
                    {"About", "약"},
                    {"aboutThis", "이 시뮬레이터 정보"},
                    {"terms","이용 약관"}
                }
            },
            {
                "tu", new Dictionary<string, string>(){
                    {"ChooseTrack", "Bir yol seçin"},
                    {"UrbanTrack1", "Kentsel Parça (1)"},
                    {"UrbanTrack2", "Kentsel Parça (2)"},
                    {"RaceTrack1", "Yarış Pisti (1)"},
                    {"RaceTrack2", "Yarış Pisti (2)"},
                    {"CameraCalibration", "Kamera Kalibrasyonu"},
                    {"Exit", "çıkış"},
                    {"Status", "Durum"},
                    {"SteeringAngle", "Direksiyon açısı"},
                    {"Speed", "Hız (KPH))"},
                    {"Sensors", "Sensörler"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Sunucuyu Başlat"},
                    {"ServerIP", "Sunucu IP'si: 127.0.0.1"},
                    {"ServerPort", "Sunucu Bağlantı Noktası: 25001"},
                    {"LapsAndCheckpointsHeader", "Turlar ve Kontrol Noktaları"},
                    {"LapsAndCheckpoints", "Turlar ve Kontrol Noktaları \n => Turlar : {0} \n => Kontrol Noktaları: {1}/{2}"},
                    {"Lap", "Tur {0} : {1}/{2}\n"},
                    {"Configuration", "Yapılandırma"},
                    {"Obstacles", "Engeller"},
                    {"ManualControl", "Manuel kontrol"},
                    {"RightLaneCheckpoint", "Sağ Şerit Kontrol Noktası"},
                    {"VisibleSensorRay", "Görünür Sensör Işını"},
                    {"TopSpeed", "En yüksek hız"},
                    {"SensorAngle", "Merkezden Sensör Açısı:"},
                    {"Degrees", " Derece"},
                    {"Logs","Kütükler"},
                    {"StartedServerMsg","\n -> {0} PORT: {1} konumunda başladı"},
                    {"Reset","Sıfırla"},
                    {"QuitToMenu","Menüden çık"},
                    {"CloseThisPanel","Bu paneli kapat"},
                    {"InfoPanel","Infofenster"},
                    {"Help","->Yardım:" 
                        +"\n\tGörünümü döndürmek için Fareyi kullanın"
                        +"\n\tSahne etrafında hareket etmek için W A S D'yi kullanın"
                        +"\n\tSahnede yukarı ve aşağı uçmak için E ve Q'yu kullanın"
                        +"\n\tSahne etrafında hareket etmek için Okları kullanın"},
                    {"Settings", "ayarlar"},
                    {"RaceFinal", "yarış finali"},
                    {"UrbanFinal", "kentsel final"},
                    {"About", "Hakkında"},
                    {"aboutThis", "Bu simülatör hakkında"},
                    {"terms","Kullanım Şartları"}
                }
            },
            {
                "cn", new Dictionary<string, string>(){
                    {"ChooseTrack", "选择曲目"},
                    {"UrbanTrack1", "城市轨道（1)"},
                    {"UrbanTrack2", "城市轨道（2)"},
                    {"RaceTrack1", "赛马场（1）"},
                    {"RaceTrack2", "赛马场（1）"},
                    {"CameraCalibration", "相机校准"},
                    {"Exit", "出口"},
                    {"Status", "状态"},
                    {"SteeringAngle", "转向角"},
                    {"Speed", "速度（KPH）"},
                    {"Sensors", "感测器"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "启动服务器"},
                    {"ServerIP", "服务器IP：127.0.0.1"},
                    {"ServerPort", "服务器端口：25001"},
                    {"LapsAndCheckpointsHeader", "膝部和检查点"},
                    {"LapsAndCheckpoints", "膝部和检查点 \n => 圈数：{0} \n => 检查点 : {1}/{2}"},
                    {"Lap", "圈数 {0} : {1}/{2}\n"},
                    {"Configuration", "组态"},
                    {"Obstacles", "障碍物"},
                    {"ManualControl", "手动"},
                    {"RightLaneCheckpoint", "右车道检查站"},
                    {"VisibleSensorRay", "可见光传感器"},
                    {"TopSpeed", "最高速度"},
                    {"SensorAngle", "传感器与中心的夹角 "},
                    {"Degrees", " 度"},
                    {"Logs", "日志"},
                    {"StartedServerMsg","\n -> 从{0}端口开始：{1}"},
                    {"Reset","重启"},
                    {"QuitToMenu","退出菜单"},
                    {"CloseThisPanel","关闭此面板"},
                    {"InfoPanel","信息面板"},
                    {"Help","->帮助:" 
                        +"\n\t使用鼠标旋转视图"
                        +"\n\t使用W A S D在场景中移动"
                        +"\n\t使用E和Q在场景中向上和向下飞行"
                        +"\n\t使用箭头在场景中移动"},
                    {"Settings", "设定"},
                    {"RaceFinal", "决赛"},
                    {"UrbanFinal", "城市决赛"},
                    {"About", "关于"},
                    {"aboutThis", "关于此模拟器"},
                    {"terms","使用条款"}
                }
            },
            {
                "fr", new Dictionary<string, string>(){
                    {"ChooseTrack", "Choisissez une piste"},
                    {"UrbanTrack1", "Circuit urbain (1)"},
                    {"UrbanTrack2", "Circuit urbain (2)"},
                    {"RaceTrack1", "Circuit de course (1)"},
                    {"RaceTrack2", "Circuit de course (2)"},
                    {"CameraCalibration", "Calibrage de la caméra"},
                    {"Exit", "Sortie"},
                    {"Status", "Statut"},
                    {"SteeringAngle", "Angle de braquage"},
                    {"Speed", "Vitesse (KPH)"},
                    {"Sensors", "Capteurs"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Démarrer le serveur"},
                    {"ServerIP", "IP du serveur: 127.0.0.1"},
                    {"ServerPort", "Port du serveur: 25001"},
                    {"LapsAndCheckpointsHeader", "Tours et points de contrôle"},
                    {"LapsAndCheckpoints", "Tours et points de contrôle\n => Tours : {0} \n => Points de contrôle: {1}/{2}"},
                    {"Lap", "Tour {0} : {1}/{2}\n"},
                    {"Configuration", "Configuration"},
                    {"Obstacles", "Obstacles"},
                    {"ManualControl", "Contrôle manuel"},
                    {"RightLaneCheckpoint", "file de droite"},
                    {"VisibleSensorRay", "Rayon de capteur visible"},
                    {"TopSpeed", "Vitesse de pointe"},
                    {"SensorAngle", "Angle du capteur à partir du centre: "},
                    {"Degrees", " degrés"},
                    {"Logs","Journaux"},
                    {"StartedServerMsg","\n -> Démarré à {0} PORT: {1}"},
                    {"Reset","Réinitialiser"},
                    {"QuitToMenu","Quitter au menu"},
                    {"CloseThisPanel","Fermer ce panneau"},
                    {"InfoPanel","Panneau d'information"},
                    {"Help","->Aidez-moi:" 
                        +"\n\tUtilisez la souris pour faire pivoter la vue"
                        +"\n\tUtilisez W A S D pour vous déplacer dans la scène"
                        +"\n\tUtilisez E et Q pour voler de haut en bas dans la scène"
                        +"\n\tUtilisez les flèches pour vous déplacer dans la scène"},
                    {"Settings", "réglages"},
                    {"RaceFinal", "finale de la course"},
                    {"UrbanFinal", "finale urbaine"},
                    {"About", "À propos"},
                    {"aboutThis", "À propos de ce simulateur"},
                    {"terms","Conditions d'utilisation"}
                }
            },
            {
                "fa", new Dictionary<string, string>(){
                    {"ChooseTrack", "زمين را انتخاب كنيد"},
                    {"UrbanTrack1", "زمين شهری ۱"},
                    {"UrbanTrack2", "زمين شهری ۲"},
                    {"RaceTrack1", "زمين مسابقه سرعت ۱"},
                    {"RaceTrack2", "زمين مسابقه سرعت ۲"},
                    {"CameraCalibration", "كاليبراسيون دوربين"},
                    {"Exit", "خروج"},
                    {"Status", "وضعيت"},
                    {"SteeringAngle", "زاويه محور فرمان"},
                    {"Speed", "سرعت Km/h"},
                    {"Sensors", "حسگرها"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "شروع سرور"},
                    {"ServerIP", "ای پي سرور :‌۱۲۷.۰.۰.۱"},
                    {"ServerPort", "پورت سرور :‌۲۵۰۰۱"},
                    {"LapsAndCheckpointsHeader", "دور ها و چک پوينت ها"},
                    {"LapsAndCheckpoints", "Laps and Checkpoints \n => Laps : {0} \n => Checkpoints : {1}/{2}"},
                    {"Lap", "Lap {0} : {1}/{2}\n"},
                    {"Configuration", "تنظيمات"},
                    {"Obstacles", "موانع"},
                    {"ManualControl", "هدايت دستي"},
                    {"RightLaneCheckpoint", "بررسي چك پوينت سمت راست"},
                    {"VisibleSensorRay", "مشاهده محدوده حسگر ها"},
                    {"TopSpeed", "بيشترين سرعت"},
                    {"SensorAngle", "زاويه سنسور از مركز "},
                    {"Degrees", " درجه "},
                    {"Logs","گزارشات"},
                    {"StartedServerMsg","\n -> Started at {0} PORT : {1}"},
                    {"Reset","شروع مجدد"},
                    {"QuitToMenu","بازگشت به منو"},
                    {"CloseThisPanel","بستن پنل"},
                    {"InfoPanel","پنل اطلاعات"},
                    {"Help","->راهنما:" 
                        +"\n\tبرای چرخاندن نمای از ماوس استفاده كنيد"
                        +"\n\tبرای حركت در اطراف صحنه از W A S D استفاده كنيد"
                        +"\n\tاز E و Q برای جابجايي به بالا و پايين در صحنه استفاده كنيد"
                        +"\n\tبرای حركت در اطراف صحنه از جهت های صفحه كليد استفاده كنيد"},
                    {"Settings", "تنظيمات"},
                    {"RaceFinal", "زمين سرعت نهايي"},
                    {"UrbanFinal", "زمين شهري نهايي"},
                    {"About", "درباره"},
                    {"aboutThis", "درباره اين شبيه ساز"},
                    {"terms","شرايط استفاده"}
                }
            },
            {
                "sp", new Dictionary<string, string>(){
                    {"ChooseTrack", "Elige una pista"},
                    {"UrbanTrack1", "Vía urbana 1"},
                    {"UrbanTrack2","Vía urbana 2"},
                    {"RaceTrack1", "Pista de carreras 1"},
                    {"RaceTrack2", "Pista de carreras 2"},
                    {"CameraCalibration", "Calibración de la cámara"},
                    {"Exit", "Salida"},
                    {"Status", "Estado"},
                    {"SteeringAngle", "Ángulo de dirección"},
                    {"Speed", "Velocidad (KPH)"},
                    {"Sensors", "Sensores"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Iniciar servidor"},
                    {"ServerIP", "IP del servidor: 127.0.0.1"},
                    {"ServerPort", "Puerto del servidor: 25001"},
                    {"LapsAndCheckpointsHeader", "Vueltas y puntos de control"},
                    {"LapsAndCheckpoints", "Vueltas y puntos de control \n => Vueltas : {0} \n => Puntos de control : {1}/{2}"},
                    {"Lap", "Vuelta {0} : {1}/{2}\n"},
                    {"Configuration", "Configuración"},
                    {"Obstacles", "Obstáculos"},
                    {"ManualControl", "Control manual"},
                    {"RightLaneCheckpoint", "Puesto de control del carril derecho"},
                    {"VisibleSensorRay", "Rayo sensor visible"},
                    {"TopSpeed", "Velocidad máxima"},
                    {"SensorAngle", "Ángulo del sensor desde el centro"},
                    {"Degrees", " Grados"},
                    {"Logs","Reiniciar"},
                    {"StartedServerMsg","\n -> Comenzó en {0} PORT : {1}"},
                    {"Reset","Reiniciar"},
                    {"QuitToMenu","Salir al menú"},
                    {"CloseThisPanel","Cerrar este panel"},
                    {"InfoPanel","Info Panel"},
                    {"Help","->Ayuda:" 
                        +"\n\tUsa el mouse para rotar la vista"
                        +"\n\tUtilice W A S D para moverse por la escena"
                        +"\n\tUsa E y Q para volar arriba y abajo en la escena."
                        +"\n\tUsa flechas para moverte por la escena"},
                    {"Settings", "ajustes"},
                    {"RaceFinal", "final de carrera"},
                    {"UrbanFinal", "Final urbano"},
                    {"About", "Acerca de"},
                    {"aboutThis", "Sobre este simulador"},
                    {"terms","Términos de Uso"}
                }
            },
            {
                "it", new Dictionary<string, string>(){
                    {"ChooseTrack", "Scegli una traccia"},
                    {"UrbanTrack1", "Traccia urbana 1"},
                    {"UrbanTrack2", "Traccia urbana 2"},
                    {"RaceTrack1", "Circuito di gara 1"},
                    {"RaceTrack2", "Circuito di gara 2"},
                    {"CameraCalibration", "Calibrazione della fotocamera"},
                    {"Exit", "Uscita"},
                    {"Status", "Stato"},
                    {"SteeringAngle", "Angolo di sterzata"},
                    {"Speed", "Velocità (KPH)"},
                    {"Sensors", "Sensori"},
                    {"cm", " cm"},
                    {"KPH", " KPH"},
                    {"StartServer", "Avvia server"},
                    {"ServerIP", "IP del server: 127.0.0.1"},
                    {"ServerPort", "Porta server: 25001"},
                    {"LapsAndCheckpointsHeader", "Giri e checkpoint"},
                    {"LapsAndCheckpoints", "Giri e checkpoint:\n => Giri : {0} \n => Checkpoint : {1}/{2}"},
                    {"Lap", "Giro {0} : {1}/{2}\n"},
                    {"Configuration", "Configurazione"},
                    {"Obstacles", "Ostacoli"},
                    {"ManualControl", "Controllo manuale"},
                    {"RightLaneCheckpoint", "Checkpoint della corsia di destra"},
                    {"VisibleSensorRay", "Raggio del sensore visibile"},
                    {"TopSpeed", "Massima velocità"},
                    {"SensorAngle", "Angolo del sensore dal centro:"},
                    {"Degrees", " gradi"},
                    {"Logs","registri"},
                    {"StartedServerMsg","\n -> Avviato su {0} PORTA : {1}"},
                    {"Reset","Ripristina"},
                    {"QuitToMenu","Esci dal menu"},
                    {"CloseThisPanel","Chiudi questo pannello"},
                    {"InfoPanel","Pannello informazioni"},
                    {"Help","->Aiuto:" 
                        +"\n\tUsa il mouse per ruotare la vista"
                        +"\n\tUsa W A S D per muoverti nella scena"
                        +"\n\tUsa E e Q per volare su e giù nella scena"
                        +"\n\tUsa le frecce per muoverti nella scena"},
                    {"Settings", "impostazioni"},
                    {"RaceFinal", "Finale di gara"},
                    {"UrbanFinal", "Finale urbano"},
                    {"About", "di"},
                    {"aboutThis", "A proposito di questo simulatore"},
                    {"terms","Condizioni d'uso"}
                }
            }
        };
        // lang = configuration.Language;
        lang = PlayerPrefs.GetString("Language", "en");
        print(lang);
        dict = languages[lang];
    }
    public string FixArabic(string text)
    {
        // ArabicFixer messes with angle brackets and quotes:
        text = Regex.Replace(text, @"<[^>]+>", "", RegexOptions.None); // remove color. I don't know how else to fix.
        text = Regex.Replace(text, @"/({\d})/", "$1", RegexOptions.None);
        text = Regex.Replace(text, "/\"({\\d})/\"", "$1", RegexOptions.None);
        text = Regex.Replace(text, "['\"]", " ", RegexOptions.None);
        // Since we're passing {0} for formatting, we can't use hindu numbers.
        return ArabicSupport.ArabicFixer.Fix(text, showTashkeel: false, useHinduNumbers: false);
    }
}
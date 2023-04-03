parameters = ["ChooseTrack","UrbanTrack1","UrbanTrack2","RaceTrack1","RaceTrack2","CameraCalibration","Exit","Status","SteeringAngle","Speed","Sensors","cm","KPH","StartServer","ServerIP","ServerPort","LapsAndCheckpointsHeader","LapsAndCheckpoints","Lap","Configuration","Obstacles","ManualControl","RightLaneCheckpoint","VisibleSensorRay","TopSpeed","SensorAngle","Degrees","Logs","StartedServerMsg","Reset","QuitToMenu","CloseThisPanel","InfoPanel","Help","Settings","RaceFinal","UrbanFinal","About","aboutThis","terms"]

for param in parameters:
    value = str(input())
    if(value != ""):
        print("{\"" + param + "\", " + "\"" + value + "\"},")

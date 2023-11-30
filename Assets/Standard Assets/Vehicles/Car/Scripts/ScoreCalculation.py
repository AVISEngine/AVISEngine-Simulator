#Copyright 2020 @ Amirmohammd Zarif - Score Calculation
#Fira Autonomous Cars Race Score Calculation Method


stageTotalTime = 300
stageTotalCheckpoints = 8

eachPassedCheckPointScore = 100
eachSkippedCheckPointPenalty = 30

for i in range(1,100):
    print("#############################")
    for j in range(0,4):
        for k in range(0,2):
            teamTotalTime = i
            teamPassedCheckPoint = j
            
            TPenalty = eachSkippedCheckPointPenalty * (stageTotalCheckpoints - teamPassedCheckPoint)
            STime = stageTotalTime - (teamTotalTime + TPenalty)
            SCheckpoints = teamPassedCheckPoint * eachPassedCheckPointScore
            STotal = None
            if(k == 1):
                STotal = STime + SCheckpoints * 1.2
            elif(k == 0):
                STotal = STime + SCheckpoints * 1

            print("Time = " + str(i) + " | CP = " + str(j) + " | Ended = " + str(k) + " | Total score = " + str(STotal))
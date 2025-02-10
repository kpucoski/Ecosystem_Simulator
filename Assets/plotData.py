import pandas as pd
from matplotlib import pyplot as plt

columns = ["Time","Population","AverageSpeed","AverageRunSpeed","AverageVision","AverageChildren","AveragePregnancy","Deaths","Births"]
dfR = pd.read_csv("rabbit_data.csv", usecols=columns)
dfF = pd.read_csv("fox_data.csv", usecols=columns)


# Population
plt.plot(dfR.Time, dfR.Population,color="brown")
plt.plot(dfF.Time, dfF.Population,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Population Over Time")
plt.show()

# Population Change
changeR = dfR.diff().fillna(0)#.astype(int)
changeF = dfF.diff().fillna(0)#.astype(int)
plt.plot(dfR.Time, changeR.Population,color="brown")
plt.plot(dfR.Time, changeF.Population,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Population Change Over Time")
plt.show()

# dR/dt = rabbit birth rate * R − rabbit death rate ∗ R ∗ W
# dW/dt = wolf birth rate ∗ R ∗ W − wolf death rate ∗ W

rabbitDeathRate = changeR.Deaths/dfR.Population
rabbitBirthRate = changeR.Births/dfF.Population

foxDeathRate = changeF.Deaths/dfR.Population
foxBirthRate = changeF.Births/dfF.Population


dR_dt = rabbitBirthRate * dfR.Population - rabbitDeathRate * dfR.Population * dfF.Population
dF_dt = foxBirthRate * dfR.Population * dfF.Population - foxDeathRate * dfF.Population

plt.plot(dfR.Time, rabbitDeathRate, color="brown")
plt.plot(dfF.Time, foxDeathRate, color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Death Rate Over Time")
plt.show()


plt.plot(dfR.Time, rabbitBirthRate, color="brown")
plt.plot(dfF.Time, foxBirthRate, color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Birth Rate Over Time")
plt.show()


plt.plot(dfR.Time, dR_dt, color="brown")
plt.plot(dfF.Time, dF_dt, color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Change")
plt.show()


# Deaths
plt.plot(dfR.Time, dfR.Deaths,color="brown")
plt.plot(dfF.Time, dfF.Deaths,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Deaths Over Time")
plt.show()

# Births
plt.plot(dfR.Time, dfR.Births,color="brown")
plt.plot(dfF.Time, dfF.Births,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Births Over Time")
plt.show()

# Average Speed
plt.plot(dfR.Time, dfR.AverageSpeed,color="brown")
plt.plot(dfF.Time, dfF.AverageSpeed,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Average Speed")
plt.legend(["Rabbits", "Foxes"])
plt.title("Average Speed Over Time")
plt.show()

# Speed change
plt.plot(dfR.Time, changeR.AverageSpeed,color="brown")
plt.plot(dfR.Time, changeF.AverageSpeed,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Speed")
plt.legend(["Rabbits", "Foxes"])
plt.title("Speed Change Over Time")
plt.show()

# Average Vision Radius
plt.plot(dfR.Time, dfR.AverageVision,color="brown")
plt.plot(dfF.Time, dfF.AverageVision,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Average Vision Radius")
plt.legend(["Rabbits", "Foxes"])
plt.title("Average Vision Radius Over Time")
plt.show()

plt.plot(dfR.Time, changeR.AverageVision,color="brown")
plt.plot(dfR.Time, changeF.AverageVision,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Population")
plt.legend(["Rabbits", "Foxes"])
plt.title("Vision Change Over Time")
plt.show()


plt.plot(dfR.Time, dfR.AverageChildren,color="brown")
plt.plot(dfF.Time, dfF.AverageChildren,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Average Num Of Children")
plt.legend(["Rabbits", "Foxes"])
plt.title("Average Num Of Children Over Time")
plt.show()

plt.plot(dfR.Time, dfR.AveragePregnancy,color="brown")
plt.plot(dfF.Time, dfF.AveragePregnancy,color="darkorange")
plt.xlabel("Time")
plt.ylabel("Average Pregnancy")
plt.legend(["Rabbits", "Foxes"])
plt.title("Average Pregnancy Over Time")
plt.show()



# plt.bar(dfR.Time-5,dfR.Female,5,color="m")
# plt.bar(dfR.Time+5,dfR.Male,5,color="b")
# plt.xlabel("Time")
# plt.ylabel("Average Gender Split")
# plt.legend(["Female", "Male"])
# plt.title("Rabbit gender split")
# plt.show()
# 
# plt.bar(dfF.Time-5,dfF.Female,5,color="m")
# plt.bar(dfF.Time+5,dfF.Male,5,color="b")
# plt.xlabel("Time")
# plt.ylabel("Average Gender Split")
# plt.legend(["Female", "Male"])
# plt.title("Fox gender split")
# plt.show()


# Death Data

columns = ["Malnutrition","Predation","Old Age"]
dfRD = pd.read_csv("rabbit_death_data.csv", usecols=columns)
if not dfRD.eq(0).all().all():
    plt.pie(dfRD.to_numpy()[0],labels = columns,autopct='%1.2f%%')
    plt.title("Cause of Death Rabbits") 
    plt.legend()
    plt.show()

columns = ["Malnutrition","Old Age"]
dfFD = pd.read_csv("fox_death_data.csv", usecols=columns)
if not dfFD.eq(0).all().all():
    plt.pie(dfFD.to_numpy()[0],labels = columns,autopct='%1.2f%%')
    plt.title("Cause of Death Foxes")
    plt.legend()
    plt.show()






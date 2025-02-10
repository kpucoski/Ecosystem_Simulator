import pandas as pd
from matplotlib import pyplot as plt
import numpy as np

columns = ["Speed","Vision","Run","TTL"]
dfR = pd.read_csv("rabbit_all_data.csv", usecols=columns)
dfF = pd.read_csv("fox_all_data.csv", usecols=columns)


# Population
# plt.plot(dfR.shape[0],color="brown")
# plt.plot([0,1],dfF.shape[0],color="darkorange")
# plt.ylabel("Population")
# plt.legend(["Rabbits", "Foxes"])
# plt.title("Total Population")
# plt.show()

dfR.Speed = dfR.Speed.round(1) 

bin_edges = np.arange(dfR.Speed.min(), dfR.Speed.max() + 0.1, 0.1)
dfR['Speed Group'] = pd.cut(dfR.Speed, bins=bin_edges, right=True)

# Count rabbits in each speed bin
speed_counts = dfR['Speed Group'].value_counts().sort_index()

bin_labels = [f"{interval.left:.1f}" for interval in speed_counts.index]
# Reduce the number of labels shown (display every 2nd or 3rd bin)
display_step = 3  # Adjust this value to reduce crowding
bin_labels_shown = [label if i % display_step == 0 else "" for i, label in enumerate(bin_labels)]

# Plot bar chart
plt.figure(figsize=(12, 6))
plt.bar(bin_labels, speed_counts.values, color='skyblue', width=1)

# Labels and title
plt.xlabel("Speed Range (0.1 increments)")
plt.ylabel("Number of Rabbits")
plt.title("Distribution of Rabbit Speeds (0.1 Bins)")
plt.xticks(bin_labels, bin_labels_shown, rotation=45, fontsize=10)  # Show only selected labels

# Show plot
plt.show()
#print("F")
#print(dfF.corr())
#print("R")
#print(dfR.corr())
#print(dfF['Speed'].corr(dfF['Vision']))
#print(dfR['Speed'].corr(dfR['Vision']))
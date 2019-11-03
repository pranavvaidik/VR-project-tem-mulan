env_name = None  # Name of the Unity environment binary to launch
train_mode = True  # Whether to run the environment in training or inference mode

import matplotlib.pyplot as plt
import numpy as np
import sys
import cv2
import imutils

from skimage import data, color
from skimage.transform import hough_circle, hough_circle_peaks
from skimage.feature import canny
from skimage.draw import circle_perimeter
from skimage.util import img_as_ubyte
from itertools import combinations
from math import sqrt

def circ_list_condense(circ_list):
	final_circ_list = circ_list
	
	for circA, circB in list(combinations(circ_list,2)):
		
		if circA in final_circ_list and circB in final_circ_list:
			center_diff = sqrt( (circA[0]-circB[0])**2 + (circA[1]-circB[1])**2 )
			radA = circA[2]
			radB = circB[2]
			
			if center_diff <= max(radA,radB): # good enough overlap, remove smaller circle
				if radA >= radB:
					final_circ_list.remove(circA)
				else:
					final_circ_list.remove(circB)
	
	return final_circ_list

def find_buttons(image):
	#image =  cv2.imread("test_images\screen_400x400_2019-10-27_02-22-25.png") #img_as_ubyte(data.coins()[160:230, 70:270])
	img = cv2.cvtColor(image,cv2.COLOR_RGB2BGR)
	gray = cv2.cvtColor(image,cv2.COLOR_BGR2GRAY)
	edges = canny(gray, sigma=3, low_threshold=10, high_threshold=50)


	# Detect two radii
	hough_radii = np.arange(15, 35, 2)
	hough_res = hough_circle(edges, hough_radii)

	# Select the most prominent 3 circles
	accums, cx, cy, radii = hough_circle_peaks(hough_res, hough_radii,
											   total_num_peaks=8)


	circ_list = [(cy[i],cx[i],radii[i]) for i in range(len(cx))]

	circ_list = circ_list_condense(circ_list)

	rect_list = []
	#for centerX, centerY, radius in cx, cy, radii:

	#	rect_list.append((centerX-radius,centerY-radius,centerX+radius,centerY+radius))
	
	#color in BGR - yellow
	color = (0,255,255)
	
	for center_y, center_x, radius in circ_list:
		cv2.circle(img, (center_x, center_y), radius, color, 2)
		text = 'found'

	button_list = [list(circ) for circ in circ_list]
	
		
	cv2.imshow('video',img)
	
	return button_list

from mlagents.envs.environment import UnityEnvironment

#%matplotlib inline

print("Python version:")
print(sys.version)

# check Python version
if (sys.version_info[0] < 3):
	raise Exception("ERROR: ML-Agents Toolkit (v0.3 onwards) requires Python 3")






detector = cv2.SimpleBlobDetector()

print(env_name)
env = UnityEnvironment(file_name=env_name)

# Set the default brain to work with
default_brain = env.brain_names[0]
print(default_brain)
brain = env.brains[default_brain]


# Reset the environment
env_info = env.reset(train_mode=train_mode)[default_brain]

# Examine the state space for the default brain
print("Agent state looks like: \n{}".format(env_info.vector_observations[0]))

p=0
# Examine the observation space for the default brain
gray_old = None

obj_locs = None
textAction = dict()
while True:

	if obj_locs is None:
		env_info = env.step()[default_brain]
	else:
		for circ in obj_locs:
			textAction['vision_brain'] = "Button"
			env_info = env.step(circ, text_action = textAction)[default_brain]
		
	print("number of visual observations:",len(env_info.visual_observations))
	
	
	for observation in env_info.visual_observations:
		action = None
		

		
		
		obs = list(env_info.visual_observations[0][0])
		
		
		obj_locs = find_buttons((np.array(obs)*255).astype('uint8'))
		print(obj_locs)
		
		
		
			
		print("works so far")
		k = cv2.waitKey(2)
		#k=k+1
	if k==27:
		cv2.destroyAllWindows()
		break
	#gray_old = gray
	p=p+1
	print("did this work?")

	
env.close()
		
		
		

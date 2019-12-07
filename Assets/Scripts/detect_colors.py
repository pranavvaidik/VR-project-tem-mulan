env_name = None  # Name of the Unity environment binary to launch
train_mode = True  # Whether to run the environment in training or inference mode

import matplotlib.pyplot as plt
import numpy as np
import sys
import cv2
#import imutils

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
											   total_num_peaks=15)


	circ_list = [(cy[i],cx[i],radii[i]) for i in range(len(cx)) if accums[i] > 0.18]

	circ_list = circ_list_condense(circ_list)

	rect_list = []
	#for centerX, centerY, radius in cx, cy, radii:

	#	rect_list.append((centerX-radius,centerY-radius,centerX+radius,centerY+radius))
	
	all_buttons = find_button_colors(img, circ_list)
	
	#color in BGR - yellow
	color = (0,255,255)
	
	
	
	for center_y, center_x, radius in circ_list:
		cv2.circle(img, (center_x, center_y), radius, color, 2)
		text = 'found'

	
	
	# remove these lines eventually
	# all_buttons = dict()
	# for circ in circ_list:
		# all_buttons[circ] = "Blue"
	
	button_list = [list(circ) for circ in circ_list]
	
		
	cv2.imshow('video',img)
	
	return all_buttons
	
def find_button_colors(image, button_list):
	
	#mask = np.zeros(shape = image.shape, dtype = 'uint8')
	
	
	
	buttons = dict()
	
	for center_y, center_x, radius in button_list:
		
		mask = np.zeros(shape = image.shape, dtype = 'uint8')
		cv2.circle(mask, (center_x, center_y), radius, color = (255,255,255), thickness = -1)
		
		mask_gray = cv2.cvtColor(image,cv2.COLOR_BGR2GRAY)
		
		masked_image = cv2.bitwise_and(src1 = image, src2 = mask)
		
		
		bgr = np.sum(np.sum(masked_image,axis = 1), axis = 0 )/(np.count_nonzero(masked_image)/3)
		
		red = bgr[2]
		green = bgr[1]
		blue = bgr[0]
		
		color = None
		
		
		if blue/green > 1.5:
			if green/red > 0.8:
				color = "Blue"
		
		if green/red > 1.5:
			if red/blue > 0.8:
				color = "Green"
			else:
				if green/blue > 1.5:
					color = "Green"

		if red/blue > 1.5:
			if blue/green > 0.8:
				color = "Red"				
		
		
		if red/green >= 0.8 and red/green <= 1.2:
			
			if blue/green > 1.5:
				color = "Blue"
			elif blue/green < 0.7:
				color = "Yellow"
				
		if blue/green >= 0.8 and blue/green <= 1.2:
			
			if red/green > 1.5:
				color = "Red"
			elif red/green < 0.7:
				color = "Cyan"
				
		if blue/red >= 0.8 and blue/red <= 1.2:
			if green/blue > 1.5:
				color = "Green"
			elif green/blue < 0.7:
				color = "Purple"
		
		# if red > 170:
			
			# if green < 140:
			
				# if blue < 140:
					# color = 'Red'
				# elif blue > 170:
					# color = 'Purple'
			
			# else:
				# if green < 150:
					# if blue < 120:
						# color = 'Orange'
						
				# elif green > 170:
					
					# if blue < 120:
						# color = "Yellow"
					# elif blue > 170:
						# color = "White" # also faint gray
					
		
		# elif red < 170:
			
			# if green < 140:
				# if blue > 170:
					# color = 'Blue'
				# elif blue < 140:
					# color = 'Gray'
			
			# elif green > 170:
				# if blue > 170:
					# color = 'Cyan'
				# elif blue < 140:
					# color = 'Green'
		
		if color is None:
			color = 'Unassigned Color'
		
		if color != 'Gray':
			buttons[(center_y, center_x, radius)] = color
		
		print(bgr, color)
		
		
		
		#plt.imshow(cv2.cvtColor(masked_image,cv2.COLOR_BGR2RGB))
		#plt.show()
	
	return buttons

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
image_size = 1000
while True:

	if not obj_locs:
		env_info = env.step()[default_brain]
	else:
		for i, circ in enumerate(obj_locs):
			
			button_color = obj_locs[circ]
		
			textAction['vision_brain'] = button_color[0]
			env_info = env.step(list(circ)+[len(obj_locs)-i-1], text_action = textAction)[default_brain]
			#env_info = env.step([i/image_size for i in circ]+[len(obj_locs)-i-1], text_action = textAction)[default_brain]
		
	print("number of visual observations:",len(env_info.visual_observations))
	
	
	for observation in env_info.visual_observations:
		action = None
		
		
		
		obs = list(env_info.visual_observations[0][0])
		
		image_size = len(obs)
		
		obj_locs = find_buttons((np.array(obs)*255).astype('uint8'))
		print(obj_locs)
		
		
		
			
		print("works so far")
		k = cv2.waitKey(2)
		
	#if k==27:
	#	cv2.destroyAllWindows()
	#	break
	#gray_old = gray
	p=p+1
	print("did this work?")

cv2.destroyAllWindows()	
env.close()
		
		
		

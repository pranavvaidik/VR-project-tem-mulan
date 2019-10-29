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
	hough_radii = np.arange(5, 35, 2)
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
		cv2.circle(image, (center_x, center_y), radius, color, 2)
		text = 'found'

	# Draw them
	# fig, ax = plt.subplots(ncols=1, nrows=1, figsize=(10, 4))
	# image = color.gray2rgb(image)
	# for center_y, center_x, radius in circ_list:#zip(cy, cx, radii):
		# circy, circx = circle_perimeter(center_y, center_x, radius,
										# shape=image.shape)
		# image[circy, circx] = (220, 20, 20)
		
	cv2.imshow('video',image)

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
while True:
	env_info = env.step()[default_brain]
	print("number of visual observations:",len(env_info.visual_observations))
	
	
	for observation in env_info.visual_observations:
		action = None
		

		
		
		obs = list(env_info.visual_observations[0][0])
		
		# convert BGR image to RGB
		#RGB_img = np.array([[obs[i][j][::-1] for j in range(len(obs))] for i in range(len(obs[0]))])
		
		# convert image to grayscale
		#gray = cv2.cvtColor(RGB_img.astype('float32'), cv2.COLOR_BGR2GRAY)
		#gray = gray*255
		
		#thresh = 18
		#edges = cv2.Canny(gray.astype('uint8'),thresh,thresh*3)
		
		find_buttons(np.array(obs)*255))
		
		
		#img = (RGB_img*255).astype('uint8')
		
		# #cv2.imshow('video',img)
		# if p%10 == 1:
			# #take a screenshot
			# a = int((p-1)/10)
			# name = 'example_'+str(a)+'.jpg'
			# cv2.imwrite(name,img)
			
		# print("got till here")
		# if gray_old is None:
			# cv2.imshow('video',gray)#gray.astype('uint8'))
		# else:
			# diff = cv2.absdiff(gray,gray_old)
			# temp = cv2.threshold(diff.astype('uint8'), 25,255,cv2.THRESH_BINARY)[1]
			
			# temp = cv2.dilate(temp, None, iterations=2)
			# cnts = cv2.findContours(temp.copy(), cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_SIMPLE)
			# cnts = imutils.grab_contours(cnts)
			
			# for c in cnts:
				
				# # compute the bounding box for the contour, draw it on the frame,
				# # and update the text
				# (x, y, w, h) = cv2.boundingRect(c)
				# cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 1)
				# text = "Occupied"
			
			#cv2.imshow('video',img)
			
		
			
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
		
		
		

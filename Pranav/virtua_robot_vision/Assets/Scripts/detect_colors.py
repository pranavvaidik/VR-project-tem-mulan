env_name = None  # Name of the Unity environment binary to launch
train_mode = True  # Whether to run the environment in training or inference mode

import matplotlib.pyplot as plt
import numpy as np
import sys
import cv2
import imutils

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

print("it works till here!")
# Set the default brain to work with
default_brain = env.brain_names[1]
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
	print(len(env_info.visual_observations))
	for observation in env_info.visual_observations:
		action = None
		

		
		
		obs = list(env_info.visual_observations[0][0])
		
		#print(obs[0][0].shape)
		RGB_img = np.array([[obs[i][j][::-1] for j in range(len(obs))] for i in range(len(obs[0]))])#cv2.cvtColor(obs[0][0], cv2.COLOR_BGR2RGB)
		
		#print(len(RGB_img),len(RGB_img[0]),len(RGB_img[0][0]),RGB_img[0][0])
		
		gray = cv2.cvtColor(RGB_img.astype('float32'), cv2.COLOR_BGR2GRAY)
		gray = gray*255
		
		#blurred = cv2.GaussianBlur(gray, (5, 5), 0)
		#thresh = cv2.threshold(blurred, 60, 255, cv2.THRESH_BINARY)[1]
		
		thresh = 18
		edges = cv2.Canny(gray.astype('uint8'),thresh,thresh*3)
		
		#keypoints = detector.detect(gray.astype('uint8'))
		print(gray.astype('uint8')[50][50])
		#img = cv2.drawKeypoints(img, keypoints, np.array([]), (0,0,255), cv2.DRAW_MATCHES_FLAGS_DRAW_RICH_KEYPOINTS)
		
		img = (RGB_img*255).astype('uint8')
		
		
		if gray_old is None:
			cv2.imshow('video',gray)#gray.astype('uint8'))
		else:
			diff = cv2.absdiff(gray,gray_old)
			temp = cv2.threshold(diff.astype('uint8'), 25,255,cv2.THRESH_BINARY)[1]
			
			temp = cv2.dilate(temp, None, iterations=2)
			cnts = cv2.findContours(temp.copy(), cv2.RETR_EXTERNAL,cv2.CHAIN_APPROX_SIMPLE)
			cnts = imutils.grab_contours(cnts)
			
			for c in cnts:
				
				# compute the bounding box for the contour, draw it on the frame,
				# and update the text
				(x, y, w, h) = cv2.boundingRect(c)
				cv2.rectangle(img, (x, y), (x + w, y + h), (0, 255, 0), 1)
				text = "Occupied"
			
			
			cv2.imshow('video',img)
		print("works so far")
		k = cv2.waitKey(2)
		#k=k+1
	if k==27:
		cv2.destroyAllWindows()
		break
	gray_old = gray
	p=p+1
	print(p)

	
env.close()
		
		
		

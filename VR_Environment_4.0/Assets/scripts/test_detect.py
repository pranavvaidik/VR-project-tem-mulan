import numpy as np
import matplotlib.pyplot as plt
import cv2
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

# Load picture and detect edges
image =  cv2.imread("test_images\screen_400x400_2019-10-27_02-22-25.png") #img_as_ubyte(data.coins()[160:230, 70:270])
img = cv2.cvtColor(image,cv2.COLOR_RGB2BGR)
image = cv2.cvtColor(image,cv2.COLOR_BGR2GRAY)
edges = canny(image, sigma=3, low_threshold=10, high_threshold=50)


# Detect two radii
hough_radii = np.arange(20, 35, 2)
hough_res = hough_circle(edges, hough_radii)

# Select the most prominent 3 circles
accums, cx, cy, radii = hough_circle_peaks(hough_res, hough_radii,
                                           total_num_peaks=8)


circ_list = [(cy[i],cx[i],radii[i]) for i in range(len(cx))]

circ_list = circ_list_condense(circ_list)

rect_list = []
#for centerX, centerY, radius in cx, cy, radii:

#	rect_list.append((centerX-radius,centerY-radius,centerX+radius,centerY+radius))



# Draw them
fig, ax = plt.subplots(ncols=1, nrows=1, figsize=(10, 4))
image = color.gray2rgb(image)
for center_y, center_x, radius in circ_list:#zip(cy, cx, radii):
    circy, circx = circle_perimeter(center_y, center_x, radius,
                                    shape=image.shape)
    image[circy, circx] = (220, 20, 20)

ax.imshow(image, cmap=plt.cm.gray)
plt.show()






def intersectionOverUnion(boxA, boxB):
	# determine the (x, y)-coordinates of the intersection rectangle
	xA = max(boxA[0], boxB[0])
	yA = max(boxA[1], boxB[1])
	xB = min(boxA[2], boxB[2])
	yB = min(boxA[3], boxB[3])
 
	# compute the area of intersection rectangle
	interArea = max(0, xB - xA + 1) * max(0, yB - yA + 1)
 
	# compute the area of both the prediction and ground-truth
	# rectangles
	boxAArea = (boxA[2] - boxA[0] + 1) * (boxA[3] - boxA[1] + 1)
	boxBArea = (boxB[2] - boxB[0] + 1) * (boxB[3] - boxB[1] + 1)
 
	# compute the intersection over union by taking the intersection
	# area and dividing it by the sum of prediction + ground-truth
	# areas - the interesection area
	iou = interArea / float(boxAArea + boxBArea - interArea)
 
	# return the intersection over union value
	return iou
	
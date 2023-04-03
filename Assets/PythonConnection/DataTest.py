import struct
import re

msg = "<image>askdlasjldjals</image><sensor>as;dkalskd</sensor>"

imageTagCheck = re.search('<image>(.*?)<\/image>', msg, re.IGNORECASE)
if(imageTagCheck):
	image = imageTagCheck.group(1)
	print(image)
#print([(m.start(0), m.end(0)) for m in re.finditer("<image>(.*?)<\/image>", msg)])


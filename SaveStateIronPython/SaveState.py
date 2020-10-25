f = open('C:/Users/KramRul/source/repos/SPNET_LB1/SaveStateIronPython/IronPyRead.txt', 'rt')
print f
fileLines = f.readlines()
f = open('C:/Users/KramRul/source/repos/SPNET_LB1/SaveStateIronPython/IronPyWrite.txt', 'r+')
print f
f.write('This is a test\n')
fileLinesWrite = f.readlines()
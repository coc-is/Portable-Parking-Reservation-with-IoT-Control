bten=0
ir=1
stat=2
motrun=3
motdir=4
breksw=5
limup=6
limdown=7
buzz=8

threshold=300

messag=""
lasmessag=""
sendnext=false
solarstream=false
solarindex=1
sendindex=1
sent=0
novehiclecount=0
vehiclecount=0
hold=false

solardat={}
recorded=0


function lift()
if gpio.read(limup)==0 --and not((gpio.read(ir)==0) and (adc.read(0)<threshold))
then
gpio.write(motdir,0)
gpio.write(motrun,0)
end
up=true

end

function drop()
if gpio.read(limdown)==0
then
gpio.write(motdir,1)
gpio.write(motrun,0)
end
up=false

end

function liftchek(level)

if up
then

if level==1
then

if lasmessag=="00001"
then
print("A00003E")
lasmessag=""
end

gpio.write(motrun,1)

else
gpio.write(motdir,0)
gpio.write(motrun,0)
end

end


end

function dropchek(level)

if not up
then

if level==1
then

if lasmessag=="00001"
then
print("A00003E")
lasmessag=""
end

gpio.write(motrun,1)

else
gpio.write(motdir,1)
gpio.write(motrun,0)
end

end


end

function vehidetec()


if (gpio.read(ir)==0) and (adc.read(0)<threshold)
then
novehiclecount=0
vehiclecount=vehiclecount+1

else
novehiclecount= novehiclecount +1
vehiclecount=0

end

if novehiclecount>2
then

if (not up) and (not hold)
then

lift()
novehiclecount=0

end

elseif vehiclecount>2
then

hold=false
vehiclecount=0

end


end

function operation(data)

if data=="00001"
then

hold=true
lasmessag=data
if up
then
drop()
else
lift()
end


elseif data=="00002"
then

lasmessag=data
solarstream=true
sendindex=solarindex+1
if sendindex>144
then
sendindex=1
end
sendnext=true
sent=0
record()

elseif data=="00000"
then

operation(lasmessag)

elseif data=="00003"
then
sendnext=true;

elseif data==""
then

return

else

print("A00000E")


end






end



function solarsend()

if solarstream and sendnext
then


num=solardat[sendindex]
if num<10
then
num="000"..tostring(num)

elseif num<100
then
num="00"..tostring(num)

elseif num<1000
then

num="0"..tostring(num)

else
num=tostring(num)

end


print("A1"..num.."E")

sendnext=false
sendindex=sendindex+1
sent=sent+1

if sendindex>144
then
sendindex=1

end

if sent>=144
then
solarstream=false

end


end

end

function record()

file.seek("set",0)

file.writeline(tostring(solarindex))
for i=1,#solardat,1
do
file.writeline(tostring(solardat[i]))
end
file.flush()

end

function retriev()

file.seek("set",0)

solarindex=tonumber(file.readline())

for i=1,144,1
do
solardat[i]=tonumber(file.readline())

end



end

function sample()

solardat[solarindex]=adc.read(0)
solarindex=solarindex+1

if solarindex>144
then
solarindex=1
end

recorded=recorded+1

if recorded>=24
then
recorded=0
record()
end




end

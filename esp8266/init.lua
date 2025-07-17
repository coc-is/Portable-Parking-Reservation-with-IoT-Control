local idletime=10000

require("def")

-- 0=in 1=out 2=int 3=opendrain
gpio.mode(bten,1)

for i=1,2,1
do
gpio.mode(i,0)
gpio.mode(i+2,1)
end

for i=5,7,1
do
gpio.mode(i,0)
end

gpio.mode(buzz,1)

gpio.write(bten,0)
gpio.write(motrun,1)--1 stop 0 run
gpio.write(buzz,0)

up=gpio.read(limup)==1

tmr.create():alarm(idletime,tmr.ALARM_SINGLE,function() require("setup") end)

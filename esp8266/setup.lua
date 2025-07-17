
uart.setup(0, 115200, 8, uart.PARITY_NONE, uart.STOPBITS_1, 0)
count=0
--print(uart.getconfig(0))
function parse(data)

for i=1,#data,1
do

char=string.sub(data,i,i)
if char=='A'
then
messag=''

elseif char=='E'
then

operation(messag)

else
messag=messag..char

end


end


end


if file.open("solar.dat","r+") 
then
else
file.open("solar.dat","w+")
file.writeline("1")
for i=1,144,1
do
file.writeline("0")
end
file.flush()
end

retriev()

uart.on("data",0,function(data) parse(data) end,0)

gpio.trig(limup,"both",liftchek)
gpio.trig(limdown,"both",dropchek)

tmr.create():alarm(10000,tmr.ALARM_AUTO, vehidetec)
tmr.create():alarm(7,tmr.ALARM_AUTO, solarsend)
tmr.create():alarm(600000,tmr.ALARM_AUTO, sample)


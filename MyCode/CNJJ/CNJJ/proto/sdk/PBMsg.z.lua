err: unwarper.py assert isinstance(else_warp_out, nodes.EndWarp), Block indices are unreliable while you are mangling them! P.S. Probably they should not be named indices... But they ARE used as indices during other phases. Sometimes.
local PBMsg = class("PBMsg")
local Util = require("sdk.util.Util")
PBMsg.pb_ = nil
PBMsg.tag_ = nil
PBMsg.pbHashCode_ = 0
local PBC_VM_TAG_DEAFULT = "default"
local PBC_VM_TAG_CHAT = "chat"
PBMsg.ctor = function (self, pbFile, msgFile, tag)
	JJLog.i("ctor, pbFile=", pbFile, ", msgFile=", msgFile)

	self.tag_ = tag or PBC_VM_TAG_DEAFULT
	self.pbHashCode_ = jl.FastHash:hashString(msgFile)
	self.pb_ = require(pbFile)

	if Util:getJlichtVersion() < 5150100 then
		local size = 1
		local buf, size = CCFileUtils:sharedFileUtils():getFileData(msgFile, "rb", size)

		if buf then
			JJLog.i("buf ~= nil, size=", size)

			buf = string.sub(buf, 1, size)

			self.pb_.register(buf)
			JJLog.i("after register, err=", self.pb_.lasterror())
		else
			JJLog.i("buf == nil")
		end
	end

	return 
end
PBMsg.encode = function (self, msg)
	if PBC_VM_TAG_CHAT == self.tag_ then
		return self.pb_.encode("cn.jj.chatservice.msg.protocol.PB_ChatSlaveReqMsg", msg)
	else
		return self.pb_.encode("cn.jj.service.msg.protocol.TKMobileReqMsg", msg)
	end

	return 
end
PBMsg.decode = function (self, buf, len)
	local msg = false
	local err = nil

	if PBC_VM_TAG_CHAT == self.tag_ then
		msg, err = self.pb_.decode("cn.jj.chatservice.msg.protocol.PB_ChatSlaveAckMsg", buf, len)
	else
		msg, err = self.pb_.decode("cn.jj.service.msg.protocol.TKMobileAckMsg", buf, len)
	end

	if msg == false then
		JJLog.i("decode, err=" .. err)

		msg = nil
	end

	return msg
end
PBMsg.getBuffer = function (self, buf, len)
	return self.pb_.getLightuserdataBuf(buf, len)
end
PBMsg.getPBHashCode = function (self)
	return self.pbHashCode_
end

return PBMsg

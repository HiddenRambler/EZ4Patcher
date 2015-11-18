#pragma once

class __declspec(dllimport) CRomManager
{
	private: int objects[2048]; //make the compiler allocate space for the class
public:
	CRomManager(void);
	CRomManager(const CRomManager&);
	virtual  ~CRomManager(void);
	CRomManager&  operator = (const CRomManager& manager);

	unsigned long __cdecl EnumGBC2GBAFromFiles(unsigned char *,unsigned long *,char const *,...);
	unsigned long  CRomManager::EnumGBC2GBAFromFiles(unsigned char *,unsigned long *,class CStringArray &);
	unsigned long __cdecl CRomManager::EnumNES2GBAFromFiles(unsigned char *,unsigned long *,char const *,...);
	unsigned long  CRomManager::EnumNES2GBAFromFiles(unsigned char *,unsigned long *,class CStringArray &);
	void  CRomManager::SetNESCharacter(int,int,int,int,int,unsigned long);
	void  CRomManager::FormatNESPreload(int,unsigned long,char const *);
	void  CRomManager::SetPCECharacter(int,int,int,int,int,unsigned long);
	void  CRomManager::FormatPCEPreload(int,unsigned long,char const *);
	unsigned long __cdecl CRomManager::EnumPCE2GBAFromFiles(unsigned char *,unsigned long *,char const *,...);
	unsigned long  CRomManager::EnumPCE2GBAFromFiles(unsigned char *,unsigned long *,class CStringArray &);
	unsigned long  CRomManager::AddROMBomaPatch(char const *,char const *,unsigned char);
	unsigned long  CRomManager::ApplyIPSPatch(char const *,char const *);
	unsigned long  CRomManager::ApplyIPSPatch(char const *,unsigned char *,unsigned long);
	unsigned char *  CRomManager::FindMotif(unsigned char *,unsigned long,unsigned char *,unsigned long,unsigned long);
	unsigned char *  CRomManager::FindMotif(unsigned char *,unsigned long,unsigned char *,unsigned long);
	unsigned long  CRomManager::SaverPatch(unsigned char * *,unsigned long *,enum EZCARTTYPE,unsigned int,enum SAVERTYPE *,unsigned long);
	void  CRomManager::Modify1MSaverRom(unsigned char * *,unsigned long *,unsigned int);
	unsigned long  CRomManager::SpecialRomPatch(unsigned char * *,unsigned long *,enum EZCARTTYPE,unsigned long);
	int  CRomManager::HeaderValid(unsigned char *);
	void  CRomManager::FillComplementCheck(unsigned char *);
	void  CRomManager::InflateROM(unsigned char * *,unsigned long *);
	unsigned long  CRomManager::GetSaverTypeAndSize(unsigned char * *,unsigned long *,enum SAVERTYPE *);
	void  CRomManager::GetPCECharacter(int,int &,int &,int &,int &,unsigned long &);
	void  CRomManager::GetNESCharacter(int,int &,int &,int &,int &,unsigned long &);
	int  CRomManager::RemoveIntro(unsigned char *,unsigned long *);
	int  CRomManager::GetSaverSpacial(unsigned char *);
	int  CRomManager::QuerySpecial(unsigned char *);
	unsigned short  CRomManager::CRC16(unsigned char *,unsigned int);
	void  CRomManager::fixCRC(unsigned char *);
	void  CRomManager::ModifyNDSHeader(unsigned char *,unsigned long,unsigned long);
	int  CRomManager::NDSPatch1(unsigned char * &,unsigned long &,bool,unsigned short,unsigned short);
};

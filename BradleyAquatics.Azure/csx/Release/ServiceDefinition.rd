﻿<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="BradleyAquatics.Azure" generation="1" functional="0" release="0" Id="d863e4e3-338d-4ae8-928f-259f5b8ae716" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="BradleyAquatics.AzureGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="BradleyAquatics:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/LB:BradleyAquatics:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="BradleyAquatics:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/MapBradleyAquatics:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="BradleyAquaticsInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/MapBradleyAquaticsInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:BradleyAquatics:Endpoint1">
          <toPorts>
            <inPortMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquatics/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapBradleyAquatics:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquatics/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapBradleyAquaticsInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquaticsInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="BradleyAquatics" generation="1" functional="0" release="0" software="C:\Users\JB\source\repos\BradleyAquatics\BradleyAquatics.Azure\csx\Release\roles\BradleyAquatics" entryPoint="base\x64\WaHostBootstrapper.exe" parameters="base\x64\WaIISHost.exe " memIndex="-1" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;BradleyAquatics&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;BradleyAquatics&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquaticsInstances" />
            <sCSPolicyUpdateDomainMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquaticsUpgradeDomains" />
            <sCSPolicyFaultDomainMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquaticsFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyUpdateDomain name="BradleyAquaticsUpgradeDomains" defaultPolicy="[5,5,5]" />
        <sCSPolicyFaultDomain name="BradleyAquaticsFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="BradleyAquaticsInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="50789c49-017f-42ee-be0d-1f80d788fb34" ref="Microsoft.RedDog.Contract\ServiceContract\BradleyAquatics.AzureContract@ServiceDefinition">
      <interfacereferences>
        <interfaceReference Id="e157d959-879e-4f87-848d-461742324abb" ref="Microsoft.RedDog.Contract\Interface\BradleyAquatics:Endpoint1@ServiceDefinition">
          <inPort>
            <inPortMoniker name="/BradleyAquatics.Azure/BradleyAquatics.AzureGroup/BradleyAquatics:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>
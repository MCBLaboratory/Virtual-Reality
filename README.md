<h1 align="center">The Integration of Virtual Reality and EEG: A Step-by-Step Guideline</h1>
<p align="center">
  <img src="Pictures/LSL Unity.png" width="850">
</p>

<h2>Overview</h2>
<p>This project provides a comprehensive framework for integrating multimodal data streams within a Virtual Reality environment. It leverages Unity alongside the Lab Streaming Layer (LSL) protocol to synchronize data from a Varjo XR3 headset and an EEG headset, enabling researchers to collect and analyze physiological and behavioral data simultaneously.</p>

<h2>Requirements</h2>
<ul>
  <li><b>Windows PC:</b> Tested on Windows 10 22H2</li>
  <li><b>Virtual Reality:</b> Tested with a Varjo XR3 headset</li>
  <li><b>EEG:</b> Compatible EEG headset such as the Enobio 32 EEG system</li>
</ul>

<h2>Getting Started</h2>
<ol>
  <li>Clone the repository:<br>
    <code>git clone https://github.com/MCBLaboratory/Virtual-Realtity.git</code>
  </li>
  <li>Open <b>Unity Hub</b>, click <b>Add → Add project from disk</b>, and select the cloned project folder.</li>
</ol>

<h2>Unity Versions & Dependencies</h2>
<ul>
  <li><b>Operating System:</b> Windows 10 or 11</li>
  <li><b>Unity Versions:</b> 
    <ul>
      <li><b>Windows PC:</b> Unity <code>2022.3.23f1</code></li>
    </ul>
  </li>
  <li><b>Required Unity Packages:</b>
    <ol>
      <li>OpenXR Plugin</li>
	  <li>OpenXR Interaction Toolkit</li>
	  <li>VarjoXR Plugin</li>
      <li>LSL4Unity</li>
      <li>Post Processing</li>
      <li>TextMeshPro</li>
    </ol>
  </li>
</ul>

<h2>Unity Installation Instructions</h2>
<ol>
  <li>Download and extract the project files or clone the repository as described above.</li>
  <li>Open <b>Unity Hub</b>, select <b>Add → Add project from disk</b>, and navigate to the <b>DataFactory Unity folder</b>.</li>
  <li><b>First launch notes:</b>
    <ul>
      <li>The <code>Library</code> folder and some packages may require manual installation.</li>
      <li>To install missing packages:
        <ul>
          <li>Go to <b>Window → Package Manager</b></li>
          <li>Add the required Unity packages listed above.</li>
        </ul>
      </li>
    </ul>
  </li>
</ol>

<h2>Development Team</h2>
<p>Development by Juriaan Wolfers, project managed by Caspar Krampe and Philip Dean, funded by the COMFOCUS EU project.</p>

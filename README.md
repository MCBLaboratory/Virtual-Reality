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
  <li><b>EEG:</b> Compatible EEG headset such as the Enobio 32 EEG system from Neuroelectrics</li>
</ul>

<h2>Getting Started</h2>
<ol>
  <li>Clone the repository:<br>
    <code>git clone https://github.com/MCBLaboratory/Virtual-Reality.git</code>
  </li>
  <li>Open <b>Unity Hub</b>, click <b>Add → Add project from disk</b>, and select the cloned project folder.</li>
</ol>

<h2>Unity Versions & Dependencies</h2>
<ul>
  <li><b>Operating System:</b> Windows 10 or 11</li>
  <li><b>Unity Versions:</b> 
    <ul>
      <li><b>Windows PC:</b> Tested with Unity <code>2022.3.23f1</code> & <code>2022.3.28f1</code</li>
    </ul>
  </li>
  <li><b>Required Unity Packages:</b>
    <ol>
      <li>OpenXR Plugin</li>
	  <li>OpenXR Interaction Toolkit</li>
	  <li>VarjoXR Plugin</li><url>https://github.com/varjocom/VarjoUnityXRPlugin</url>
      <li>LSL4Unity</li><url>https://github.com/labstreaminglayer/LSL4Unity</url>
      <li>Post Processing</li>
      <li>TextMeshPro</li>
    </ol>
  </li>
</ul>

<h2>Unity Installation Instructions</h2>
<ol>
  <li>Download and extract the project files or clone the repository as described above.</li>
  <li>Open <b>Unity Hub</b>, select <b>Add → Add project from disk</b>, and navigate to the <b>Varjo - GitHub Unity folder</b>.</li>
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

<h2>Example Scene</h2>

<p>
  This example scene simulates a product selection task involving 10 different package designs, including one designated <b>Target</b> design. The user completes <b>100 rounds</b> of selection, followed by a final round to choose their favorite package.
</p>

<p>
  The scene begins with a button labeled <b>"I am ready!"</b>. Pressing it three times starts the experiment. All packages appear blank and are interactable. After every 10 rounds, the scene automatically enters an <b>indefinite break</b> period until the user chooses to continue.
</p>

<p>
  The experiment alternates randomly between two types of condition rounds:
</p>

<ul>
  <li><b>C1:</b> 80% Target-Normal vs. 20% Normal-Normal</li>
  <li><b>C2:</b> 20% Target-Normal vs. 80% Normal-Normal</li>
</ul>

<p>Each condition type runs for 50 rounds, evenly splitting the total of 100 rounds.</p>

<h3>Included Scripts</h3>
<ul>
  <li><b>MainFile.cs:</b> Controls experiment flow, round distribution, event logging, and CSV output</li>
  <li><b>GrabAdvance.cs:</b> Handles object grabbing and material updates for package designs</li>
  <li><b>EyeTrackingVarjo.cs:</b> Modified Varjo eye-tracking script with added variable logging</li>
  <li><b>StartMainGame.cs:</b> Manages the start of the main task, countdown, and event triggers</li>
  <li><b>StartFavoriteProduct.cs:</b> Initiates the final favorite selection round with event logging</li>
  <li><b>GrabFavoriteProduct.cs:</b> Handles selection logic in the final round and logs results</li>
  <li><b>Floating.cs:</b> Applies floating animations to UI elements</li>
  <li><b>ButtonPressed.cs:</b> Plays a sound when any UI button is clicked</li>
</ul>

<h2>Miscellaneous</h2>
<ol>
  <li>
    The project is configured for a <strong>Varjo</strong> headset and
    <strong>Valve Index Controllers</strong>.
  </li>
  <li>
    To change the VR headset settings (e.g., add Meta Quest 3), disable
    <strong>Varjo</strong> and enable <strong>OpenXR</strong> in
    <em>Project Settings → XR Plug-in Management</em>.
  </li>
  <li>
    To update the controller configuration, navigate to
    <em>Project Settings → XR Plug-in Management → OpenXR</em> and replace the
    <strong>Valve Index Controller Profile</strong> with another (e.g., Meta Quest controllers)
    using the <code>+</code> / <code>−</code> icons.
  </li>
  <li>
    The <strong>OpenXR Simulator</strong> can be enabled to use keyboard and mouse controls:
    <ul>
      <li>In the Hierarchy, enable <em>Setup → XR Device Simulator</em>.</li>
      <li>
        In the Game view, press the <code>+</code> on the simulator’s control window to view
        the input mappings.
      </li>
      <li>
        Navigation may feel unintuitive at first, but the on-screen guide helps clarify the
        control scheme.
      </li>
    </ul>
  </li>
</ol>

<h2>Development Team</h2>
<p>Development by Juriaan Wolfers, project managed by Caspar Krampe and Philip Dean, funded by the COMFOCUS EU project.</p>

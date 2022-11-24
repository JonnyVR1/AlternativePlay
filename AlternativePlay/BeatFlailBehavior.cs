using AlternativePlay.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AlternativePlay
{
    public class BeatFlailBehavior : MonoBehaviour
    {
        private const float BallMass = 3.0f;
        private const float LinkMass = 1.0f;
        private const float HandleMass = 2.0f;
        private const float AngularDrag = 2.0f;
        private const int LinkCount = 3;
        private readonly Pose leftHiddenPose = new Pose(new Vector3(-1.0f, -1000.0f, 0.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));
        private readonly Pose rightHiddenPose = new Pose(new Vector3(1.0f, -1000.0f, 0.0f), Quaternion.Euler(90.0f, 0.0f, 0.0f));

        private List<GameObject> leftPhysicsFlail;
        private List<GameObject> rightPhysicsFlail;

        private List<GameObject> leftLinkMeshes;
        private List<GameObject> rightLinkMeshes;
        private GameObject leftHandleMesh;
        private GameObject rightHandleMesh;

        /// <summary>
        /// To be invoked every time when starting the GameCore scene.
        /// </summary>
        public void BeginGameCoreScene()
        {
            // Do nothing if we aren't playing Flail
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatFlail) { return; }

            TrackedDeviceManager.instance.LoadTrackedDevices();

            var config = Configuration.instance.ConfigurationData;
            if (config.LeftFlailMode != BeatFlailMode.None)
            {
                Utilities.CheckAndDisableForTrackerTransforms(config.LeftFlailTracker);
            }

            if (config.RightFlailMode != BeatFlailMode.None)
            {
                Utilities.CheckAndDisableForTrackerTransforms(config.RightFlailTracker);
            }

            StartCoroutine(DisableSaberMeshes());
        }

        private void Awake()
        {
            // Do nothing if we aren't playing Flail
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatFlail) { return; }

            // Create the GameObjects used for physics calculations
            var config = Configuration.instance.ConfigurationData;
            leftPhysicsFlail = CreatePhysicsChain("Left", config.LeftFlailLength / 100.0f);
            rightPhysicsFlail = CreatePhysicsChain("Right", config.RightFlailLength / 100.0f);
            leftLinkMeshes = Utilities.CreateLinkMeshes(leftPhysicsFlail.Count, config.LeftFlailLength / 100.0f);
            leftHandleMesh = Instantiate(BehaviorCatalog.instance.AssetLoaderBehavior.FlailHandlePrefab);
            rightLinkMeshes = Utilities.CreateLinkMeshes(rightPhysicsFlail.Count, config.RightFlailLength / 100.0f);
            rightHandleMesh = Instantiate(BehaviorCatalog.instance.AssetLoaderBehavior.FlailHandlePrefab);
        }

        private void FixedUpdate()
        {
            // Do nothing if we aren't playing Flail
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatFlail) { return; }

            var config = Configuration.instance.ConfigurationData;
            float gravity = config.FlailGravity * -9.81f;

            switch(config.LeftFlailMode)
            {
                default:
                case BeatFlailMode.Flail:
                    // Apply gravity to the left handle
                    foreach (var link in leftPhysicsFlail.Skip(1))
                    {
                        var rigidBody = link.GetComponent<Rigidbody>();
                        rigidBody.AddForce(new Vector3(0, gravity, 0) * rigidBody.mass);
                    }

                    // Apply motion force from the left controller
                    var leftFirstLink = leftPhysicsFlail.First();
                    Pose leftSaberPose = BehaviorCatalog.instance.SaberDeviceManager.GetLeftSaberPose(config.LeftFlailTracker);
                    leftFirstLink.transform.position = leftSaberPose.position * 10.0f;
                    leftFirstLink.transform.rotation = leftSaberPose.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;

                case BeatFlailMode.Sword:
                case BeatFlailMode.None:
                    // Do nothing
                    break;
            }

            switch (config.RightFlailMode)
            {
                default:
                case BeatFlailMode.Flail:
                    // Apply gravity to the right handle
                    foreach (var link in rightPhysicsFlail.Skip(1))
                    {
                        var rigidBody = link.GetComponent<Rigidbody>();
                        rigidBody.AddForce(new Vector3(0, gravity, 0) * rigidBody.mass);
                    }

                    // Apply motion force from the right controller
                    var rightFirstLink = rightPhysicsFlail.First();
                    Pose rightSaberPose = BehaviorCatalog.instance.SaberDeviceManager.GetRightSaberPose(config.RightFlailTracker);
                    rightFirstLink.transform.position = rightSaberPose.position * 10.0f;
                    rightFirstLink.transform.rotation = rightSaberPose.rotation * Quaternion.Euler(0.0f, 90.0f, 0.0f);
                    break;

                case BeatFlailMode.Sword:
                case BeatFlailMode.None:
                    // Do nothing
                    break;
            }
        }

        private void Update()
        {
            // Do nothing if we aren't playing Flail
            if (Configuration.instance.ConfigurationData.PlayMode != PlayMode.BeatFlail) { return; }

            var config = Configuration.instance.ConfigurationData;
            switch (config.LeftFlailMode)
            {
                default:
                case BeatFlailMode.Flail:
                    // Move saber to the last link
                    var lastLeftLink = leftPhysicsFlail.Last();
                    Pose leftLastLinkPose = new Pose(lastLeftLink.transform.position / 10.0f, lastLeftLink.transform.rotation * Quaternion.Euler(0.0f, -90.0f, 180.0f));
                    BehaviorCatalog.instance.SaberDeviceManager.SetLeftSaberPose(leftLastLinkPose);

                    // Move all links into place
                    Utilities.MoveLinkMeshes(leftLinkMeshes, leftPhysicsFlail, (float)config.LeftFlailLength / 100f);

                    // Move handle based on the original saber position
                    Pose leftSaberPose = BehaviorCatalog.instance.SaberDeviceManager.GetLeftSaberPose(config.LeftFlailTracker);
                    float oneChainDistance = config.LeftFlailLength / 100.0f / (leftPhysicsFlail.Count - 1);
                    Vector3 moveHandleUp = leftSaberPose.rotation * new Vector3(0.0f, 0.0f, oneChainDistance); // Move handle forward one chain length
                    leftHandleMesh.transform.position = leftSaberPose.position + moveHandleUp;
                    leftHandleMesh.transform.rotation = leftSaberPose.rotation;
                    break;

                case BeatFlailMode.Sword:
                    // Do nothing
                    break;

                case BeatFlailMode.None:
                    // Remove the sword
                    BehaviorCatalog.instance.SaberDeviceManager.SetLeftSaberPose(leftHiddenPose);
                    break;
            }

            switch (config.RightFlailMode)
            {
                default:
                case BeatFlailMode.Flail:
                    // Move saber to the last link
                    var lastRightLink = rightPhysicsFlail.Last();
                    Pose rightLastLinkPose = new Pose(lastRightLink.transform.position / 10.0f, lastRightLink.transform.rotation * Quaternion.Euler(0.0f, -90.0f, 180.0f));
                    BehaviorCatalog.instance.SaberDeviceManager.SetRightSaberPose(rightLastLinkPose);

                    // Move all links into place
                    Utilities.MoveLinkMeshes(rightLinkMeshes, rightPhysicsFlail, config.RightFlailLength / 100.0f);

                    // Move handle based on the original saber position
                    Pose rightSaberPose = BehaviorCatalog.instance.SaberDeviceManager.GetRightSaberPose(config.RightFlailTracker);
                    float oneChainDistance = config.RightFlailLength / 100.0f / (rightPhysicsFlail.Count - 1);
                    Vector3 moveHandleUp = rightSaberPose.rotation * new Vector3(0.0f, 0.0f, oneChainDistance); // Move handle forward one chain length
                    rightHandleMesh.transform.position = rightSaberPose.position + moveHandleUp;
                    rightHandleMesh.transform.rotation = rightSaberPose.rotation;
                    break;

                case BeatFlailMode.Sword:
                    // Do nothing
                    break;

                case BeatFlailMode.None:
                    // Remove the sword
                    BehaviorCatalog.instance.SaberDeviceManager.SetRightSaberPose(rightHiddenPose);
                    break;
            }
        }

        private void OnDestroy()
        {
            // Destroy all flail game objects
            if (leftPhysicsFlail != null) leftPhysicsFlail.ForEach(o => Destroy(o));
            leftPhysicsFlail = null;

            if (rightPhysicsFlail != null) rightPhysicsFlail.ForEach(o => Destroy(o));
            rightPhysicsFlail = null;

            if (leftLinkMeshes != null) leftLinkMeshes.ForEach(o => Destroy(o));
            leftLinkMeshes = null;

            if (rightLinkMeshes != null) rightLinkMeshes.ForEach(o => Destroy(o));
            rightLinkMeshes = null;

            if (leftHandleMesh != null) Destroy(leftHandleMesh);
            leftHandleMesh = null;

            if (rightHandleMesh != null) Destroy(rightHandleMesh);
            rightHandleMesh = null;
        }

        /// <summary>
        /// Creates a chain of GameObjects used for physics and connects them
        /// </summary>
        private List<GameObject> CreatePhysicsChain(string prefix, float length)
        {
            var chain = new List<GameObject>();
            var handle = Utilities.CreateLink(prefix + "FlailHandle", HandleMass, AngularDrag, true);
            chain.Add(handle);

            for (int i = 0; i < LinkCount; i++)
            {
                var link = Utilities.CreateLink(prefix + "FlailLink" + i.ToString(), LinkMass, AngularDrag);
                chain.Add(link);
            }

            var ball = Utilities.CreateLink(prefix + "FlailBall", BallMass, AngularDrag);
            chain.Add(ball);

            Utilities.ConnectChain(chain, length);
            return chain;
        }

        /// <summary>
        /// Disables the rendering of the saber
        /// </summary>
        private IEnumerator DisableSaberMeshes()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            var config = Configuration.instance.ConfigurationData;
            if (config.LeftFlailMode == BeatFlailMode.None)
            {
                BehaviorCatalog.instance.SaberDeviceManager.DisableLeftSaberMesh();
            }

            if (config.RightFlailMode == BeatFlailMode.None)
            {
                BehaviorCatalog.instance.SaberDeviceManager.DisableRightSaberMesh();
            }
        }
    }
}
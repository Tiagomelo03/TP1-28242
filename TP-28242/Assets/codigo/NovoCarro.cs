using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovoCarro : MonoBehaviour
{
    // Variáveis para armazenar as entradas do jogador e forças atuais
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking;

    // Configurações de força do motor, força de travagem e ângulo máximo de direção
    [SerializeField] private float motorForce, breakForce, maxSteerAngle;

    // Referências aos Wheel Colliders das rodas
    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    // Referências aos Transforms das rodas para atualizar a posição e rotação visual das rodas
    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;

    private void FixedUpdate()
    {
        // Chamado a cada frame de física, captura entrada do usuário e controla o carro
        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        // Captura a entrada horizontal (setas esquerda/direita ou A/D)
        horizontalInput = Input.GetAxis("Horizontal");

        // Captura a entrada vertical (setas cima/baixo ou W/S)
        verticalInput = Input.GetAxis("Vertical");

        // Captura o estado do botão de  (espaço)
        isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor()
    {
        // Aplica a força do motor às rodas dianteiras com base na entrada vertical
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        // Determina a força de travagem com base no estado do freio
        currentbreakForce = isBreaking ? breakForce : 0f;

        // Aplica os travoes
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        // Aplica a força de travagem a todas as rodas
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        // Calcula o ângulo de direção com base na entrada horizontal
        currentSteerAngle = maxSteerAngle * horizontalInput;

        // Aplica o ângulo de direção às rodas dianteiras
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        // Atualiza a posição e rotação visual de cada roda
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        // Obtém a posição e rotação do WheelCollider
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);

        // Atualiza a rotação e posição do Transform da roda para corresponder ao WheelCollider
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
}

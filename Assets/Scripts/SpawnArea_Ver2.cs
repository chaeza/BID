using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnArea_Ver2 : MonoBehaviour
{
	[Header("[��������]")]
	[SerializeField] float width = 1f; // X �� �������� ũ��
	[SerializeField] float height = 1f; // Z �� �������� ũ��
	public Vector3 getRandomPos()
	{
		Vector3 size = transform.lossyScale;
		size.x *= width;
		size.z *= height;
		// ���(��ġ�̵�, ȸ��, ������)�� �̿��ؼ� ������ġ�� ��Ȯ�� ���� ����Ѵ�.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);

		Vector3 randomPos = rMat.MultiplyPoint(new Vector3(Random.Range(-0.5f, 0.5f), transform.position.y, Random.Range(-0.5f, 0.5f)));
		//randomPos.y = transform.position.y;
		return randomPos;
	}
	// �����Ϳ����� �����ϵ���
#if UNITY_EDITOR
	// ����� �׸� �� ȣ��Ǵ� �Լ�
	private void OnDrawGizmos()
	{
		drawCube(Color.yellow);
	}

	// ����� ������ �Ǿ��� �� ȣ��Ǵ� �Լ�
	void OnDrawGizmosSelected()
	{
		drawCube(Color.green);
	}

	//
	// ������ �������� ť�� 1�� �׸���
	void drawCube(Color drawColor)
	{
		Gizmos.color = drawColor;
		Vector3 size = transform.lossyScale;
		size.x *= width;
		size.z *= height;

		// ��ġ�� ȸ���� �������� ����� ����� ���ؼ�
		// Gizmos �� �����ϸ� ���� �׸��� Cube�� ����� ����(��ġ�̵�, ȸ��, ������)�� �޴´�.
		Matrix4x4 rMat = Matrix4x4.TRS(transform.position, transform.rotation, size);
		Gizmos.matrix = rMat;
		Gizmos.DrawCube(Vector3.zero, Vector3.one);
	}
#endif // UNITY_EDITOR
}

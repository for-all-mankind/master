using UnityEngine;
using System.Collections;

public class PerlinNoise {
	const int B = 256;
	int[] m_perm = new int[B+B];

	public PerlinNoise(int seed) {
		Random.seed = seed;

		int i, j, k;
		for (i = 0 ; i < B ; i++) {
			m_perm[i] = i;
		}

		while (--i != 0) {
			j = Random.Range(0, B);

			k = m_perm[i];
			m_perm[i] = m_perm[j];
			m_perm[j] = k;
		}

		for (i = 0 ; i < B; i++) {
			m_perm[B + i] = m_perm[i];
		}
	}

	float Fade(float t) {
		return t * t * t * (t * (t * 6.0f - 15.0f) + 10.0f);
	}

	float Lerp(float t, float a, float b) {
		return a + t * (b - a);
	}

	float Grad(int hash, float x, float y) {
		int h = hash & 7;
		float u = h<4 ? x : y;
		float v = h<4 ? y : x;
		return (((h & 1) != 0) ? -u : u) + (((h & 2) != 0) ? -2.0f * v : 2.0f * v);
	}

	float Noise( float x, float y ) {
		// Returns a noise value between -0.75 and 0.75
		int ix0, iy0, ix1, iy1;
		float fx0, fy0, fx1, fy1, s, t, nx0, nx1, n0, n1;

		// Integer part
		ix0 = Mathf.FloorToInt(x);
		iy0 = Mathf.FloorToInt(y);
		// Fractional part
		fx0 = x - ix0;
		fy0 = y - iy0;

		fx1 = fx0 - 1.0f;
		fy1 = fy0 - 1.0f;

		ix1 = (ix0 + 1) & 0xff; // Wrap to 0..255
		iy1 = (iy0 + 1) & 0xff;

		ix0 = ix0 & 0xff;
		iy0 = iy0 & 0xff;

		t = Fade( fy0 );
		s = Fade( fx0 );

		nx0 = Grad(m_perm[ix0 + m_perm[iy0]], fx0, fy0);
		nx1 = Grad(m_perm[ix0 + m_perm[iy1]], fx0, fy1);

		n0 = Lerp( t, nx0, nx1 );

		nx0 = Grad(m_perm[ix1 + m_perm[iy0]], fx1, fy0);
		nx1 = Grad(m_perm[ix1 + m_perm[iy1]], fx1, fy1);

		n1 = Lerp(t, nx0, nx1);

		return 0.507f * Lerp( s, n0, n1 );
	}

	public float FractalNoise(float x, float y, int octNum, float frq, float amp) {
		float gain = 1.0f;
		float sum = 0.0f;

		for(int i = 0; i < octNum; i++) {
			sum += Noise(x*gain/frq, y*gain/frq) * amp/gain;
			gain *= 2.0f;
		}

		return sum;
	}
}














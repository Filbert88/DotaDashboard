const nextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'cdn.dota2.com',
        pathname: '/**',
      },
    ],
  },
};

export default nextConfig;

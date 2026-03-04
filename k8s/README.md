# M8ScoreTool Container/Kubernetes

## Docker

Build image from repo root:

```powershell
docker build -t m8scoretool:latest .
```

Run locally:

```powershell
docker run --rm -p 8080:8080 -e M8SCORETOOL_PASSWORD="your-password" m8scoretool:latest
```

Then open: `http://localhost:8080`

## Kubernetes

1. Push image to your registry and update `image` in `m8scoretool.yaml`.
2. Update the secret password value in `m8scoretool.yaml`.
3. Apply manifests:

```powershell
kubectl apply -f .\k8s\m8scoretool.yaml
```

4. Port-forward to test:

```powershell
kubectl port-forward svc/m8scoretool 8080:80
```

Then open: `http://localhost:8080`
